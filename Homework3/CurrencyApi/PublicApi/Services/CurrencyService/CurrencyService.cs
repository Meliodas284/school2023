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
		public async Task<Currency?> GetCurrency()
		{
			await CheckRequestsLimit();

			var client = _factory.CreateClient("currency");
			var uri = $"latest?currencies={_options.DefaultCurrency}&base_currency={_options.BaseCurrency}";

			var response = await client.GetAsync(uri);

			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadFromJsonAsync<ExternalApiResponseDto>();
				return result!.Data[_options.DefaultCurrency];
			}

			throw new Exception(response.StatusCode.ToString());
		}

