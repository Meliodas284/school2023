using InternalAPI.Models.Dtos;
using InternalAPI.Services.CacheFileService;

namespace InternalAPI.Services.CachedCurrencyAPIService;

/// <summary>
/// Реализует методы получения всех валют с использованием кэша
/// </summary>
public class CachedCurrencyAPIService : ICachedCurrencyAPIService
{
	private readonly ICacheFileService _cachedFileService;

	/// <summary>
	/// Конструктор для инициализации зависимостей
	/// </summary>
	/// <param name="cachedFileService">Сервис для работы с кэшем</param>
	public CachedCurrencyAPIService(ICacheFileService cachedFileService)
	{
		_cachedFileService = cachedFileService;
	}

	/// <summary>
	/// Получить курс валюты на дату с использованием кэша
	/// </summary>
	/// <param name="currencyType">Тип валюты (код)</param>
	/// <param name="date">Нужная дата</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Курс валюты на дату</returns>
	public async Task<CurrencyDto> GetCurrencyOnDateAsync(CurrencyType currencyType, DateOnly date, CancellationToken cancellationToken)
	{
		var currency = await _cachedFileService.GetCurrencyOnDate(currencyType, date, cancellationToken);
		return new CurrencyDto
		{
			CurrencyType = currencyType,
			Value = currency.Value
		};
	}

	/// <summary>
	/// Получить актуальный курс валюты с использованием кэша
	/// </summary>
	/// <param name="currencyType">Тип валюты (код)</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Актуальный курс валюты</returns>
	public async Task<CurrencyDto> GetCurrentCurrencyAsync(CurrencyType currencyType, CancellationToken cancellationToken)
	{
		var currency = await _cachedFileService.GetCurrency(currencyType, cancellationToken);
		return new CurrencyDto
		{
			CurrencyType = currencyType,
			Value = currency.Value
		};
	}
}
