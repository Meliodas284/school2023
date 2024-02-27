using InternalAPI.Models.Dtos;

namespace InternalAPI.Services.CachedCurrencyAPIService;

/// <summary>
/// Предоставляет методы получения курса валют с использованием кэша
/// </summary>
public interface ICachedCurrencyAPIService
{
	/// <summary>
	/// Получает текущий курс валюты с использованием кэша
	/// </summary>
	/// <param name="currencyType">Валюта, для которой необходимо получить курс</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Текущий курс</returns>
	Task<CurrencyDto> GetCurrentCurrencyAsync(CurrencyType currencyType, CancellationToken cancellationToken);

	/// <summary>
	/// Получает курс валюты, актуальный на <paramref name="date"/> с использованием кэша
	/// </summary>
	/// <param name="currencyType">Валюта, для которой необходимо получить курс</param>
	/// <param name="date">Дата, на которую нужно получить курс валют</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Курс на дату</returns>
	Task<CurrencyDto> GetCurrencyOnDateAsync(CurrencyType currencyType, DateOnly date, CancellationToken cancellationToken);
}
