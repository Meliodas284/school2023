namespace InternalAPI.Models.Dtos;

/// <summary>
/// Представляет DTO с информацией о курсе валюты на определенную дату
/// </summary>
public class DateCurrencyDto
{
	/// <summary>
	/// Дата курса
	/// </summary>
	public DateOnly Date { get; set; }

    /// <summary>
    /// Тип валюты (код)
    /// </summary>
    public CurrencyType Code { get; set; }

	/// <summary>
	/// Курс валюты
	/// </summary>
	public decimal Value { get; set; }
}
