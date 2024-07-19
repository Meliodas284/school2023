using InternalApi.Domain.Enum;

namespace InternalApi.Domain.Dto;

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