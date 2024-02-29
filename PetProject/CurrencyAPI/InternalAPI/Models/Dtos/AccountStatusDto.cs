using System.Text.Json.Serialization;

namespace InternalApi.Models.Dtos
{
    /// <summary>
    /// Представляет информацию о запросах аккаунта
    /// </summary>
    public class AccountStatusDto
    {
		/// <summary>
		/// Уникальный идентификатор аккаунта
		/// </summary>
		[JsonPropertyName("account_id")]
		public long AccountId { get; set; }

		/// <summary>
		/// Информация о квотах
		/// </summary>
		[JsonPropertyName("quotas")]
		public QuotaDto Quotas { get; set; }
    }

    /// <summary>
    /// Представляет квоты
    /// </summary>
    public class QuotaDto
    {
		/// <summary>
		/// Месячная квота
		/// </summary>
		[JsonPropertyName("month")]
		public LimitDataDto Month { get; set; }
	}

	/// <summary>
	/// Представляет информацию о лимите в квоте
	/// </summary>
	public class LimitDataDto
    {
		/// <summary>
		/// Сколько всего запросов было доступно
		/// </summary>
		[JsonPropertyName("total")]
		public int Total { get; set; }

		/// <summary>
		/// Сколько запросов было использовано
		/// </summary>
		[JsonPropertyName("used")]
		public int Used { get; set; }

		/// <summary>
		/// Сколько запросов осталось
		/// </summary>
		[JsonPropertyName("remaining")]
		public int Remaining { get; set; }
	}
}
