using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InternalAPI.Exceptions;
using InternalAPI.Models.Dtos;
using InternalAPI.Services.CachedCurrencyAPIService;
using InternalAPI.Services.CurrencyAPIService;

namespace InternalAPI.Services.GrpcServices;

/// <summary>
/// Реализует методы gRPC сервиса валют
/// </summary>
public class GrpcCurrencyService : CurrencyService.CurrencyServiceBase
{
	private readonly ICachedCurrencyAPIService _currencyService;
	private readonly ICurrencyAPIService _apiService;

	/// <summary>
	/// Конструктор, инициализирует зависимости
	/// </summary>
	/// <param name="currencyService">Сервис валют, использующий кэш</param>
	/// <param name="apiService">Сервис валют, использующий внешний API</param>
	public GrpcCurrencyService(ICachedCurrencyAPIService currencyService, ICurrencyAPIService apiService)
    {
        _currencyService = currencyService;
		_apiService = apiService;
	}

	/// <summary>
	/// Получить актуальный курс заданной валюты
	/// </summary>
	/// <param name="request">Запрос, содержащий код валюты</param>
	/// <param name="context">Контекст сервера</param>
	/// <returns>Актуальный курс валюты</returns>
	/// <exception cref="CurrencyNotFoundException"></exception>
	public override async Task<CurrencyResponse> Get(Request request, ServerCallContext context)
	{
		if (!System.Enum.TryParse(request.Code, out CurrencyType currencyCode))
		{
			throw new CurrencyNotFoundException("Нет валюты с таким кодом!");
		}

		var result = await _currencyService
				.GetCurrentCurrencyAsync(currencyCode, context.CancellationToken);

		return new CurrencyResponse
		{
			Code = request.Code,
			Value = (double)result.Value
		};
	}

	/// <summary>
	/// Получить курс заданной валюты на заданную дату
	/// </summary>
	/// <param name="request">Запрос, содержащий код валюты и дату</param>
	/// <param name="context">Контекст сервера</param>
	/// <returns>Курс валюты на дату</returns>
	/// <exception cref="CurrencyNotFoundException"></exception>
	public override async Task<CurrencyOnDateResponse> GetOnDate(OnDateRequest request, ServerCallContext context)
	{
		if (!System.Enum.TryParse(request.Code, out CurrencyType currencyCode))
		{
			throw new CurrencyNotFoundException("Нет валюты с таким кодом!");
		}

		var date = DateOnly.FromDateTime(request.Date.ToDateTime());

		var result = await _currencyService
			.GetCurrencyOnDateAsync(currencyCode, date, context.CancellationToken);

		return new CurrencyOnDateResponse
		{
			Code = request.Code,
			Value = (double)result.Value,
			Date = request.Date
		};
	}

	/// <summary>
	/// Получить настройки API
	/// </summary>
	/// <param name="request">Пустой параметр</param>
	/// <param name="context">Контекст сервера</param>
	/// <returns>Настройки API</returns>
	public override async Task<Settings> GetSettings(Empty request, ServerCallContext context)
	{
		var result = await _apiService.GetApiSettingsAsync(context.CancellationToken);

		return new Settings
		{
			BaseCurrency = result.BaseCurrency,
			AvailableQueries = result.RequestCount <= result.RequestLimit
		};
	}
}
