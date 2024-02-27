using InternalApi.Exceptions;
using InternalApi.Models;
using InternalApi.Models.Dtos;
using InternalAPI.Models.Dtos;

namespace InternalAPI.Services.CurrencyAPIService;

/// <summary>
/// Реализует логику получения курса для всех валют
/// </summary>
public class CurrencyAPIService : ICurrencyAPIService
{
	private readonly IHttpClientFactory _factory;

	/// <summary>
	/// Конструктор, инициализирует зависимости
	/// </summary>
	/// <param name="factory">Сервис для создания http клиента</param>
	public CurrencyAPIService(IHttpClientFactory factory)
        {
		_factory = factory;
	}

	/// <summary>
	/// Получить курс для всех валют, актуальный на <paramref name="date"/>
	/// </summary>
	/// <param name="baseCurrency">Базовая валюта, относительно которой необходимо получить курс</param>
	/// <param name="date">Дата, на которую нужно получить курс валют</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Список курсов валют на дату</returns>
	public async Task<CurrenciesOnDateDto> GetAllCurrenciesOnDateAsync(string baseCurrency, DateOnly date, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		
		await CheckRequestsLimit();

		var client = _factory.CreateClient("currency");
		var uri = $"historical?date={date.ToString("yyyy-MM-dd")}&base_currency={baseCurrency}";
		var response = await client.GetAsync(uri, cancellationToken);

		return await ParseCurrenciesOnDate(response);
	}

	/// <summary>
	/// Получить текущий курс для всех валют
	/// </summary>
	/// <param name="baseCurrency">Базовая валюта, относительно которой необходимо получить курс</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Список курсов валют</returns>
	public async Task<Currency[]> GetAllCurrentCurrenciesAsync(string baseCurrency, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		await CheckRequestsLimit();

		var client = _factory.CreateClient("currency");
		var uri = $"latest?base_currency={baseCurrency}";
		var response = await client.GetAsync(uri, cancellationToken);

		return await ParseCurrencies(response);
	}

	/// <summary>
	/// Проверяет превышение лимита запросов, вызывает исключение если превышен
	/// </summary>
	/// <exception cref="ApiRequestLimitException">Исключение при превышении лимита</exception>
	private async Task CheckRequestsLimit()
	{
		var client = _factory.CreateClient("currency");

		var accountStatus = await client
			.GetFromJsonAsync<AccountStatusDto>("status");

		if (accountStatus!.Quotas.Month.Remaining == 0)
			throw new ApiRequestLimitException();
	}

	/// <summary>
	/// Спарсить ответ в массив курсов
	/// </summary>
	/// <param name="response">Ответ внешнего API</param>
	/// <returns>Массив курсов валют</returns>
	private async Task<Currency[]> ParseCurrencies(HttpResponseMessage response)
	{
		var result = await response.Content.ReadFromJsonAsync<ExternalApiResponseDto>();
		return result!.Data.Values.ToArray();
	}

	/// <summary>
	/// Спарсить ответ в dto <see cref="CurrenciesOnDateDto"/>
	/// </summary>
	/// <param name="response">Ответ внешнего API</param>
	/// <returns>Dto <see cref="CurrenciesOnDateDto"/></returns>
	private async Task<CurrenciesOnDateDto> ParseCurrenciesOnDate(HttpResponseMessage response)
	{
		var result = await response.Content.ReadFromJsonAsync<ExternalApiResponseDto>();
		return new CurrenciesOnDateDto
		{
			LastUpdateAt = result!.Meta.LastUpdatedAt,
			Currencies = result.Data.Values.ToArray()
		};
	}
}
