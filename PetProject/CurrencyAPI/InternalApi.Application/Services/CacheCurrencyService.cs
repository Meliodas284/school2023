﻿using InternalApi.Domain.Dto;
using InternalApi.Domain.Enum;
using InternalApi.Domain.Interfaces.Services;
using InternalApi.Domain.Settings;
using Microsoft.Extensions.Options;

namespace InternalApi.Application.Services;

/// <summary>
/// Реализует методы получения всех валют с использованием кэша
/// </summary>
public class CacheCurrencyService : ICacheCurrencyService
{
	private readonly ICacheFileService _cachedFileService;
	private readonly CurrencyOptions _options;

	/// <summary>
	/// Конструктор для инициализации зависимостей
	/// </summary>
	/// <param name="cachedFileService">Сервис для работы с кэшем</param>
	/// <param name="options">Конфигурация API</param>
	public CacheCurrencyService(
		ICacheFileService cachedFileService, 
		IOptionsSnapshot<CurrencyOptions> options)
	{
		_cachedFileService = cachedFileService;
		_options = options.Value;
	}

	/// <summary>
	/// Получить курс валюты на дату с использованием кэша
	/// </summary>
	/// <param name="currencyType">Тип валюты (код)</param>
	/// <param name="date">Нужная дата</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Курс валюты на дату</returns>
	public async Task<CurrencyDto> GetCurrencyOnDateAsync(CurrencyType currencyType, DateOnly date, CancellationToken cancellationToken)
	{
		var currency = await _cachedFileService.GetCurrencyOnDate(currencyType, date, cancellationToken);
		return new CurrencyDto
		{
			CurrencyType = currencyType,
			Value = Math.Round(currency.Value, _options.CurrencyRoundCount)
		};
	}

	/// <summary>
	/// Получить актуальный курс валюты с использованием кэша
	/// </summary>
	/// <param name="currencyType">Тип валюты (код)</param>
	/// <param name="cancellationToken">Токен отмены</param>
	/// <returns>Актуальный курс валюты</returns>
	public async Task<CurrencyDto> GetCurrentCurrencyAsync(CurrencyType currencyType, CancellationToken cancellationToken)
	{
		var currency = await _cachedFileService.GetCurrency(currencyType, cancellationToken);
		return new CurrencyDto
		{
			CurrencyType = currencyType,
			Value = Math.Round(currency.Value, _options.CurrencyRoundCount)
		};
	}
}