namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Dtos
{
    /// <summary>
    /// Представляет информацию об запросах аккаунта
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
}
