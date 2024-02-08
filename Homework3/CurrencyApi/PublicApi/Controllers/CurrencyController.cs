using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Services.CurrencyService;
using Microsoft.AspNetCore.Mvc;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Controllers;

/// <summary>
/// Методы для работы с валютами
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CurrencyController : ControllerBase
{
	private readonly ICurrencyService _currencyService;

	/// <summary>
	/// Конструктор инициализирует внедренные зависимости
	/// </summary>
	/// <param name="currencyService">Сервис валют</param>
	public CurrencyController(ICurrencyService currencyService)
    {
		_currencyService = currencyService;
	}

	/// <summary>
	/// Получить курс валюты по умолчанию
	/// </summary>
	/// <returns>Информация о валюте <see cref="Currency"/></returns>
	[HttpGet]
	public async Task<ActionResult<Currency?>> GetCurrency()
	{
		var result = await _currencyService.GetCurrency();
		return Ok(result);
	}
}
