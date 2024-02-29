using InternalApi.Models;

namespace InternalAPI.Models.Dtos;

/// <summary>
/// Представляет DTO валют на определенную дату
/// </summary>
public class CurrenciesOnDateDto
{
    /// <summary>
    /// Дата курса для валют
    /// </summary>
    public DateTime LastUpdateAt { get; set; }

	/// <summary>
	/// Массив валют с курсом на определенную дату
	/// </summary>
	public Currency[] Currencies { get; set; }
}
