using InternalAPI.Exceptions;
using InternalAPI.Models;
using InternalAPI.Models.Dtos;
using InternalAPI.Services.CurrencyAPIService;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text.Json;

namespace InternalAPI.Services.CacheFileService;

/// <summary>
/// Реализует методы работы с файлами кэша
/// </summary>
public class CacheFileService : ICacheFileService
{
	private const string DirectoryPath = "Cache";
	private readonly ICurrencyApiService _currencyApiService;
	private readonly CurrencyOptions _options;

        /// <summary>
	/// Конструктор для инициализации зависимостей
	/// </summary>
	/// <param name="options">Конфигурация</param>
	/// <param name="currencyAPIService">Сервис для работы с внешним API</param>
	public CacheFileService(
		IOptionsSnapshot<CurrencyOptions> options, 
		ICurrencyApiService currencyAPIService)
        {
		_options = options.Value;
		_currencyApiService = currencyAPIService;
        }

        /// <summary>
	/// Получает курс валюты по заданному типу из файла кэша.
	/// Если нужного файла кэша нет, обращается к внешнему API и сохраняет кэш.
	/// </summary>
	/// <param name="type">Тип валюты (код)</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Курс нужной валюты</returns>
	/// <exception cref="CurrencyNotFoundException">Если не найдена валюта заданного типа</exception>
	public async Task<Currency> GetCurrency(CurrencyType type, CancellationToken cancellationToken)
	{
		var files = GetCachedFiles(DirectoryPath);
		var data = files != null ? await GetRecentCache(files, cancellationToken) : null;

		if (data == null)
		{
			data = await GetFromApi(cancellationToken);
			var dateNow = DateTime.UtcNow.ToString("yyyy-MM-ddTHH-mm-ssZ");
			await SaveCacheJsonAsync($"{DirectoryPath}/{dateNow}.json", data, cancellationToken);
		}

		var currency = data.FirstOrDefault(c => c.Code == type.ToString());
		if (currency == null)
			throw new CurrencyNotFoundException("Нет валюты с таким кодом! ");

		return currency;
	}

	/// <summary>
	/// Получает курс валюты по заданному типу из файла кэша на указанную дату.
	/// Если нужного файла кэша нет, обращается к внешнему API и сохраняет кэш.
	/// </summary>
	/// <param name="type">Тип валюты (код)</param>
	/// <param name="date">Нужная дата</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Курс нужной валюты на определенную дату</returns>
	/// <exception cref="CurrencyNotFoundException">Если не найдена валюта заданного типа</exception>
	public async Task<Currency> GetCurrencyOnDate(CurrencyType type, DateOnly date, CancellationToken cancellationToken)
	{		
		var files = GetCachedFiles(DirectoryPath);
		var data = files != null ? await GetCacheOnDate(files, date, cancellationToken) : null;

		if (data == null)
		{
			var response = await GetFromApiOnDate(date, cancellationToken);
			var fileDate = response.Item1.ToString("yyyy-MM-ddTHH-mm-ssZ");
			data = response.Item2;
			await SaveCacheJsonAsync($"{DirectoryPath}/{fileDate}.json", data, cancellationToken);
		}

		var currency = data.FirstOrDefault(c => c.Code == type.ToString());
		if (currency == null)
			throw new CurrencyNotFoundException("Нет валюты с таким кодом! ");

		return currency;
	}

	/// <summary>
	/// Получает все файлы директории кэша
	/// </summary>
	/// <param name="directoryPath">Путь к директории</param>
	/// <returns>null: если в директории нет файлов
	/// массив файлов: если есть файлы</returns>
	/// <exception cref="DirectoryNotFoundException">Если не найдена директория кэша</exception>
	private FileInfo[]? GetCachedFiles(string directoryPath)
	{		
		var directory = new DirectoryInfo(directoryPath);
		if (!directory.Exists)
			throw new DirectoryNotFoundException("Не найдена директория кэша");

		var files = directory.GetFiles("*.json");
		if (files.Length == 0)
			return null;

		return files;
	}

	/// <summary>
	/// Получает данные из самого раннего файла кэша (не старее 2 часов от текущей даты)
	/// </summary>
	/// <param name="files">Коллекция файлов кэша</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>null: нет файла не старее 2 часов; 
	/// коллекцию курсов валют если есть не устаревший файл кэша</returns>
	private async Task<Currency[]?> GetRecentCache(FileInfo[] files, CancellationToken cancellationToken)
	{		
		var now = DateTime.UtcNow;

		var recentFile = files.FirstOrDefault(f =>
		{
			cancellationToken.ThrowIfCancellationRequested();
			var fileDate = DateTime.ParseExact(
				Path.GetFileNameWithoutExtension(f.Name),
				"yyyy-MM-ddTHH-mm-ssZ",
				CultureInfo.InvariantCulture,
				DateTimeStyles.RoundtripKind);
			
			return Math.Abs((now - fileDate).TotalHours) <= 2;
		});

		if (recentFile == null)
			return null;

		using (var stream = recentFile.OpenRead())
		{
			return await JsonSerializer.DeserializeAsync<Currency[]>(stream, cancellationToken: cancellationToken);
		}
	}

	/// <summary>
	/// Получает данные из файла кэша с указанной датой.
	/// Если файлов несколько, данные получает из самого раннего файла.
	/// </summary>
	/// <param name="files">Коллекция файлов кэша</param>
	/// <param name="date">Нужная дата</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>null: если нет файлов с указанной датой;
	/// коллекцию курсов валют из файла кэша с указанной датой</returns>
	private async Task<Currency[]?> GetCacheOnDate(FileInfo[] files, DateOnly date, CancellationToken cancellationToken)
	{		
		var filesOnDate = files.Where(f =>
		{
			cancellationToken.ThrowIfCancellationRequested();
			var fileDate = DateTime.ParseExact(
				Path.GetFileNameWithoutExtension(f.Name),
				"yyyy-MM-ddTHH-mm-ssZ",
				CultureInfo.InvariantCulture,
				DateTimeStyles.RoundtripKind);

			return DateOnly.FromDateTime(fileDate) == date;
		});

		if (!filesOnDate.Any())
			return null;

		var recentFile = filesOnDate.MaxBy(f =>
		{
			cancellationToken.ThrowIfCancellationRequested();
			var fileDate = DateTime.ParseExact(
				Path.GetFileNameWithoutExtension(f.Name),
				"yyyy-MM-ddTHH-mm-ssZ",
				CultureInfo.InvariantCulture, 
				DateTimeStyles.RoundtripKind);

			return fileDate;
		});

		if (recentFile == null)
			return null;

		using (var stream = recentFile.OpenRead())
		{
			return await JsonSerializer.DeserializeAsync<Currency[]>(stream, cancellationToken: cancellationToken);
		}
	}

	/// <summary>
	/// Получает данные о курсе валют по API через сервис
	/// </summary>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Данные о курсе валют</returns>
	private async Task<Currency[]> GetFromApi(CancellationToken cancellationToken)
	{
		return await _currencyApiService
			.GetAllCurrentCurrenciesAsync(_options.BaseCurrency, cancellationToken);
	}

	/// <summary>
	/// Получает данные о курсе валют на определенную дату по API через сервис
	/// </summary>
	/// <param name="date">Нужная дата</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Данные о курсе валют на определенную дату</returns>
	private async Task<(DateTime, Currency[])> GetFromApiOnDate(DateOnly date, CancellationToken cancellationToken)
	{
		var data = await _currencyApiService
			.GetAllCurrenciesOnDateAsync(_options.BaseCurrency, date, cancellationToken);
		
		return (data.LastUpdateAt, data.Currencies);
	}

	/// <summary>
	/// Сохраняет кэш в JSON
	/// </summary>
	/// <param name="path">Путь файла</param>
	/// <param name="data">Данные кэша</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns></returns>
	private async Task SaveCacheJsonAsync(string path, Currency[] data, CancellationToken cancellationToken)
	{
		using (var stream = new FileStream(path, FileMode.Create))
		{
			await JsonSerializer.SerializeAsync(stream, data, cancellationToken: cancellationToken);
		}
	}
}
