using InternalAPI.Models;
using InternalAPI.Models.Dtos;

namespace InternalAPI.Services.CurrencyAPIService;

/// <summary>
/// Предоставляет методы получения курса всех валют
/// </summary>
public interface ICurrencyAPIService
{
	/// <summary>
	/// Получить текущий курс для всех валют
	/// </summary>
	/// <param name="baseCurrency">Базовая валюта, относительно которой необходимо получить курс</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Список курсов валют</returns>
	Task<Currency[]> GetAllCurrentCurrenciesAsync(string baseCurrency, CancellationToken cancellationToken);

	/// <summary>
	/// Получить курс для всех валют, актуальный на <paramref name="date"/>
	/// </summary>
	/// <param name="baseCurrency">Базовая валюта, относительно которой необходимо получить курс</param>
	/// <param name="date">Дата, на которую нужно получить курс валют</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Список курсов валют на дату</returns>
	Task<CurrenciesOnDateDto> GetAllCurrenciesOnDateAsync(string baseCurrency, DateOnly date, CancellationToken cancellationToken);

	/// <summary>
	/// Получить настройки API
	/// </summary>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns></returns>
	Task<ApiSettingsDto> GetApiSettingsAsync(CancellationToken cancellationToken);
}
