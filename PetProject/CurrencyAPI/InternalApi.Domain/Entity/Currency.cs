using System.Text.Json.Serialization;

namespace InternalApi.Domain.Entity;

/// <summary>
/// Класс представляющий валюту
/// </summary>
public class Currency
{
	/// <summary>
	/// Код валюты
	/// </summary>
	[JsonPropertyName("code")]
	public string Code { get; set; }

	/// <summary>
	/// Курс валюты
	/// </summary>
	[JsonPropertyName("value")]
	public decimal Value { get; set; }
}
