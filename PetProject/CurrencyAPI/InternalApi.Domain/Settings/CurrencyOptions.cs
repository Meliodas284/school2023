﻿namespace InternalApi.Domain.Settings;

/// <summary>
/// Представляет конфигурацию api через паттерн IOptions
/// </summary>
public class CurrencyOptions
{
	/// <summary>
	/// Базовый адрес запроса
	/// </summary>
	public string BaseUrl { get; init; } = string.Empty;

	/// <summary>
	/// Валюта по умолчанию
	/// </summary>
	public string DefaultCurrency { get; init; } = string.Empty;

	/// <summary>
	/// Базовая валюта
	/// </summary>
	public string BaseCurrency { get; init; } = string.Empty;

	/// <summary>
	/// Значение округления курса по умолчанию
	/// </summary>
	public int CurrencyRoundCount { get; init; }
}