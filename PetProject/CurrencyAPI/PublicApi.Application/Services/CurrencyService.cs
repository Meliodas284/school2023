using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using PublicApi.Application.Proto;
using PublicApi.Domain.Dto;
using PublicApi.Domain.Entity;
using PublicApi.Domain.Exceptions;
using PublicApi.Domain.Interfaces.Services;
using PublicApi.Domain.Settings;
using static PublicApi.Application.Proto.CurrencyService;

namespace PublicApi.Application.Services;

/// <summary>
/// Сервис по получению курса валют
/// </summary>
public class CurrencyService : ICurrencyService
{
	private readonly CurrencyApiOptions _options;
	private readonly CurrencyServiceClient _currencyServiceClient;

	/// <summary>
	/// Конструктор, инициализирует зависимости
	/// </summary>
	/// <param name="options">Конфигурация api</param>
	/// <param name="currencyServiceClient">Сервис клиента gRPC</param>
	public CurrencyService(
		IOptionsSnapshot<CurrencyApiOptions> options, 
		CurrencyServiceClient currencyServiceClient)
    {
        _options = options.Value;
		_currencyServiceClient = currencyServiceClient;
	}

	/// <summary>
	/// Получить курс валюты по умолчанию
	/// </summary>
	/// <returns>Информация о валюте <see cref="Currency"/></returns>
	/// <exception cref="CurrencyNotFoundException">При некорректном коде валюты</exception>
	public async Task<Currency> GetCurrency()
	{
		await CheckRequestsLimit();

		var request = new Request
		{
			Code = _options.DefaultCurrency
		};
		var response = await _currencyServiceClient.GetAsync(request);

		return new Currency
		{
			Code = response.Code,
			Value = response.Value
		};
	}

	/// <summary>
	/// Получить курс валюты по нужному коду
	/// </summary>
	/// <param name="code">Код валюты</param>
	/// <returns>Информацию о валюте по нужному коду <see cref="Currency"/></returns>
	/// <exception cref="CurrencyNotFoundException">При некорректном коде валюты</exception>
	public async Task<Currency> GetCurrencyByCode(string code)
	{
		await CheckRequestsLimit();

		var request = new Request
		{
			Code = code
		};
		var response = await _currencyServiceClient.GetAsync(request);

		return new Currency
		{
			Code = response.Code,
			Value = response.Value
		};
	}

	/// <summary>
	/// Получить курс валюты по коду и дате
	/// </summary>
	/// <param name="date">Дата курса</param>
	/// <param name="code">Код валюты</param>
	/// <returns>Информацию о валюте с нужным кодом
	/// и на определенную дату<see cref="Currency"/></returns>
	/// <exception cref="CurrencyNotFoundException">При некорректном коде валюты</exception>
	public async Task<DateCurrencyDto> GetCurrencyOnDate(DateOnly date, string code)
	{
		await CheckRequestsLimit();

		var datetime = date.ToDateTime(TimeOnly.MinValue).ToUniversalTime();

		var request = new OnDateRequest
		{
			Code = code,
			Date = Timestamp.FromDateTime(datetime)
		};
		var response = await _currencyServiceClient.GetOnDateAsync(request);

		return new DateCurrencyDto
		{
			Code = code,
			Date = date,
			Value = response.Value
		};
	}

	/// <summary>
	/// Получить настройки API
	/// </summary>
	/// <returns>Информацию о настройках API</returns>
	public async Task<ApiSettingsDto> GetSettings()
	{
		var settings = await _currencyServiceClient.GetSettingsAsync(new Empty());

		return new ApiSettingsDto
		{
			DefaultCurrency = _options.DefaultCurrency,
			BaseCurrency = settings.BaseCurrency,
			NewRequestAvailable = settings.AvailableQueries,
			CurrencyRoundCount = _options.CurrencyRoundCount
		};
	}

	/// <summary>
	/// Проверяет превышение лимита запросов, вызывает исключение если превышен
	/// </summary>
	/// <exception cref="ApiRequestLimitException">Исключение при превышении лимита</exception>
	private async Task CheckRequestsLimit()
	{
		var settings = await _currencyServiceClient.GetSettingsAsync(new Empty());

		if (!settings.AvailableQueries)
			throw new ApiRequestLimitException();
	}
}
