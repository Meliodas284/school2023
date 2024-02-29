namespace InternalAPI.Models.Dtos;

/// <summary>
/// Представляет DTO с информацией о курсе валюты
/// </summary>
public class CurrencyDto
{
	/// <summary>
	/// Тип валюты (код)
	/// </summary>
	public CurrencyType CurrencyType { get; set; }

	/// <summary>
	/// Значение курса
	/// </summary>
    public decimal Value { get; set; }
}

/// <summary>
/// Представляет типы валют (коды)
/// </summary>
public enum CurrencyType
{
	/// <summary>
	/// Доллар
	/// </summary>
	USD,

	/// <summary>
	/// Рубль
	/// </summary>
	RUB,

	/// <summary>
	/// Тенге
	/// </summary>
	KZT
}
