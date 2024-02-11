namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Dtos
{
    /// <summary>
    /// Представляет информацию о запросах аккаунта
    /// </summary>
    public class AccountStatusDto
    {
        /// <summary>
        /// Уникальный идентификатор аккаунта
        /// </summary>
        public long AccountId { get; set; }

		/// <summary>
		/// Информация о квотах
		/// </summary>
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
		public int Total { get; set; }

		/// <summary>
		/// Сколько запросов было использовано
		/// </summary>
		public int Used { get; set; }

		/// <summary>
		/// Сколько запросов осталось
		/// </summary>
		public int Remaining { get; set; }
	}
}
