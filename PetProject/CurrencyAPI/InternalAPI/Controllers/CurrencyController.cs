using InternalAPI.Models.Dtos;
using InternalAPI.Services.CachedCurrencyAPIService;
using InternalAPI.Services.CurrencyAPIService;
using Microsoft.AspNetCore.Mvc;

namespace InternalAPI.Controllers
{
	/// <summary>
	/// Контроллер валют
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class CurrencyController : ControllerBase
	{
		private readonly ICacheCurrencyService _currencyCacheService;
		private readonly ICurrencyApiService _currencyService;

        /// <summary>
		/// Конструктор, инициализирует зависимости
		/// </summary>
		/// <param name="currencyCacheService">Сервис валют с использованием кэша</param>
		/// <param name="currencyService">Сервис валют с использованием внешнего API</param>
		public CurrencyController(
			ICacheCurrencyService currencyCacheService, 
			ICurrencyApiService currencyService)
        {
			_currencyCacheService = currencyCacheService;
			_currencyService = currencyService;
		}

		/// <summary>
		/// Получить актуальный курс заданной валюты
		/// </summary>
		/// <param name="type">Тип валюты (код)</param>
		/// <param name="token">Токен отмены</param>
		/// <returns>Курс валюты</returns>
		[HttpGet]
		public async Task<ActionResult<CurrencyDto>> GetCurrency(CurrencyType type, CancellationToken token)
		{
			var result = await _currencyCacheService.GetCurrentCurrencyAsync(type, token);

			return Ok(result);
		}

		/// <summary>
		/// Получить курс заданной валюты на дату
		/// </summary>
		/// <param name="type">Тип валюты (код)</param>
		/// <param name="date">Дата курса</param>
		/// <param name="token">Токен отмены</param>
		/// <returns>Курс валюты на дату</returns>
		[HttpGet("{date:datetime}")]
		public async Task<ActionResult<CurrencyDto>> GetCurrencyOnDate(CurrencyType type, DateTime date, CancellationToken token)
		{
			var dateOnly = DateOnly.FromDateTime(date);
			var result = await _currencyCacheService.GetCurrencyOnDateAsync(type, dateOnly, token);

			return Ok(new DateCurrencyDto
			{
				Code = result.CurrencyType,
				Value = result.Value,
				Date = dateOnly
			});
		}

		/// <summary>
		/// Получить настройки
		/// </summary>
		/// <param name="token">Токен отмены</param>
		/// <returns>Настройки API</returns>
		[HttpGet("settings")]
		public async Task<ActionResult<ApiSettingsDto>> GetSettings(CancellationToken token)
		{
			var result = await _currencyService.GetApiSettingsAsync(token);

			return Ok(result);
		}
	}
}
