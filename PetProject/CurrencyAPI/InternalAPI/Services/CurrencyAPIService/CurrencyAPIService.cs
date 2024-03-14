using InternalAPI.Exceptions;
using InternalAPI.Models;
using InternalAPI.Models.Dtos;
using Microsoft.Extensions.Options;
using System.Net;

namespace InternalAPI.Services.CurrencyAPIService;

/// <summary>
/// Реализует логику получения курса для всех валют
/// </summary>
public class CurrencyApiService : ICurrencyApiService
{
	private readonly IHttpClientFactory _factory;
	private readonly CurrencyOptions _options;

	/// <summary>
	/// Конструктор, инициализирует зависимости
	/// </summary>
	/// <param name="factory">Сервис для создания http клиента</param>
	/// <param name="options">Настройки</param>
	public CurrencyApiService(IHttpClientFactory factory, IOptionsSnapshot<CurrencyOptions> options)
    {
		_factory = factory;
		_options = options.Value;
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
		await CheckRequestsLimit(cancellationToken);

		var client = _factory.CreateClient("currency");
		var uri = $"historical?date={date.ToString("yyyy-MM-dd")}&base_currency={baseCurrency}";
		var response = await client.GetAsync(uri, cancellationToken);

		if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
			throw new CurrencyNotFoundException();

		return await ParseCurrenciesOnDate(response, cancellationToken);
	}

	/// <summary>
	/// Получить текущий курс для всех валют
	/// </summary>
	/// <param name="baseCurrency">Базовая валюта, относительно которой необходимо получить курс</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Список курсов валют</returns>
	public async Task<Currency[]> GetAllCurrentCurrenciesAsync(string baseCurrency, CancellationToken cancellationToken)
	{
		await CheckRequestsLimit(cancellationToken);

		var client = _factory.CreateClient("currency");
		var uri = $"latest?base_currency={baseCurrency}";
		var response = await client.GetAsync(uri, cancellationToken);

		if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
			throw new CurrencyNotFoundException();

		return await ParseCurrencies(response, cancellationToken);
	}

	/// <summary>
	/// Получить настройки API
	/// </summary>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Настройки API <see cref="ApiSettingsDto"/></returns>
	public async Task<ApiSettingsDto> GetApiSettingsAsync(CancellationToken cancellationToken)
	{
		var client = _factory.CreateClient("currency");

		var accountStatus = await client
			.GetFromJsonAsync<AccountStatusDto>("status", cancellationToken);

		return new ApiSettingsDto
		{
			DefaultCurrency = _options.DefaultCurrency,
			BaseCurrency = _options.BaseCurrency,
			RequestLimit = accountStatus!.Quotas.Month.Total,
			RequestCount = accountStatus.Quotas.Month.Used,
			CurrencyRoundCount = _options.CurrencyRoundCount
		};
	}

	/// <summary>
	/// Проверяет превышение лимита запросов, вызывает исключение если превышен
	/// </summary>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <exception cref="ApiRequestLimitException">Исключение при превышении лимита</exception>
	private async Task CheckRequestsLimit(CancellationToken cancellationToken)
	{
		var client = _factory.CreateClient("currency");

		var accountStatus = await client
			.GetFromJsonAsync<AccountStatusDto>("status", cancellationToken);

		if (accountStatus!.Quotas.Month.Remaining == 0)
			throw new ApiRequestLimitException();
	}

	/// <summary>
	/// Спарсить ответ в массив курсов
	/// </summary>
	/// <param name="response">Ответ внешнего API</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Массив курсов валют</returns>
	private async Task<Currency[]> ParseCurrencies(HttpResponseMessage response, CancellationToken cancellationToken)
	{
		var result = await response.Content.ReadFromJsonAsync<ExternalApiResponseDto>(cancellationToken);
		return result!.Data.Values.ToArray();
	}

	/// <summary>
	/// Спарсить ответ в dto <see cref="CurrenciesOnDateDto"/>
	/// </summary>
	/// <param name="response">Ответ внешнего API</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Dto <see cref="CurrenciesOnDateDto"/></returns>
	private async Task<CurrenciesOnDateDto> ParseCurrenciesOnDate(HttpResponseMessage response, CancellationToken cancellationToken)
	{
		var result = await response.Content.ReadFromJsonAsync<ExternalApiResponseDto>(cancellationToken);
		return new CurrenciesOnDateDto
		{
			LastUpdateAt = result!.Meta.LastUpdatedAt,
			Currencies = result.Data.Values.ToArray()
		};
	}
}
