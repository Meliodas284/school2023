using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Dtos;
using Microsoft.Extensions.Options;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services.CurrencyService
{
    /// <summary>
    /// Сервис по получению курса валют
    /// </summary>
    public class CurrencyService : ICurrencyService
	{
		private readonly CurrencyAPIOptions _options;
		private readonly IHttpClientFactory _factory;

        /// <summary>
		/// Конструктор, инициализирует зависимости
		/// </summary>
		/// <param name="options">Конфигурация api</param>
		/// <param name="factory">Сервис для создания http клиента</param>
		public CurrencyService(IOptions<CurrencyAPIOptions> options, IHttpClientFactory factory)
        {
            _options = options.Value;
			_factory = factory;
		}

		/// <summary>
		/// Получить курс валюты по умолчанию
		/// </summary>
		/// <returns>Информация о валюте <see cref="Currency"/></returns>
		/// <exception cref="CurrencyNotFoundException">При некорректном коде валюты</exception>
		public async Task<Currency> GetCurrency()
		{
			await CheckRequestsLimit();

			var client = _factory.CreateClient("currency");
			var uri = $"latest?currencies={_options.DefaultCurrency}&base_currency={_options.BaseCurrency}";

			var response = await client.GetAsync(uri);

			if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
				throw new CurrencyNotFoundException();

			return await ParseCurrency(response, _options.DefaultCurrency);
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
			
			var client = _factory.CreateClient("currency");
			var uri = $"latest?currencies={code}&base_currency={_options.BaseCurrency}";

			var response = await client.GetAsync(uri);

			if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
				throw new CurrencyNotFoundException();

			return await ParseCurrency(response, code);
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
			
			var client = _factory.CreateClient("currency");
			var uri = $"historical?currencies={code}&date={date.ToString("yyyy-MM-dd")}&base_currency={_options.BaseCurrency}";

			var response = await client.GetAsync(uri);

			if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
				throw new CurrencyNotFoundException();

			var currency = await ParseCurrency(response, code);

			return new DateCurrencyDto 
			{
				Code = currency.Code, 
				Value = currency.Value, 
				Date = date 
			};
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
		/// Получает нужную валюту из ответа внешнего API по коду
		/// </summary>
		/// <param name="response">Ответ внешнего API</param>
		/// <param name="code">Код нужной валюты</param>
		/// <returns>Нужная валюта с кодом <see cref="Currency.Code"/></returns>
		private async Task<Currency> ParseCurrency(HttpResponseMessage response, string code)
		{
			var result = await response.Content.ReadFromJsonAsync<ExternalApiResponseDto>();
			var currency = result!.Data[code];
			currency.Value = Math.Round(currency.Value, _options.DefaultRate);

			return currency;
		}
	}
}
