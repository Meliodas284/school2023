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

