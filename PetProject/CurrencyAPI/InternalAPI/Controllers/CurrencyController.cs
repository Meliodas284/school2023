﻿using InternalApi.Models;
using InternalApi.Models.Dtos;
using InternalApi.Services.CurrencyService;
using Microsoft.AspNetCore.Mvc;

namespace InternalApi.Controllers;

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
	public async Task<ActionResult<Currency>> GetCurrency()
	{
		var result = await _currencyService.GetCurrency();
		return Ok(result);
	}

	/// <summary>
	/// Получить курс валюты по коду
	/// </summary>
	/// <param name="code">Код валюты</param>
	/// <returns>Информация о валюте по нужному коду <see cref="Currency"/></returns>
	[HttpGet("{code:regex([[A-Z]]{{3}})}")]
	public async Task<ActionResult<Currency>> GetCurrencyByCode(string code)
	{
		var result = await _currencyService.GetCurrencyByCode(code);
		return Ok(result);
	}

	/// <summary>
	/// Получить курс валюты по коду и дате
	/// </summary>
	/// <param name="date">Дата курса</param>
	/// <param name="code">Код валюты</param>
	/// <returns>Информацию о валюте с нужным кодом
	/// и на определенную дату<see cref="Currency"/></returns>
	[HttpGet("{date:datetime}/{code:regex([[A-Z]]{{3}})}")]
	public async Task<ActionResult<DateCurrencyDto>> GetCurrencyOnDate(DateTime date, string code)
	{
		var dateOnly = DateOnly.FromDateTime(date);
		var result = await _currencyService.GetCurrencyOnDate(dateOnly, code);
		return Ok(result);
	}

	/// <summary>
	/// Получить информацию о настройках API
	/// </summary>
	/// <returns>Информацию о настройках API <see cref="ApiSettingsDto"/></returns>
	[HttpGet("settings")]
	public async Task<ActionResult<ApiSettingsDto>> GetSettings()
	{
		var result = await _currencyService.GetSettings();
		return Ok(result);
	}
}
