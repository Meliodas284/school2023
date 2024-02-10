namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models
{
	/// <summary>
    /// Представляет информацию о настройках API
    /// </summary>
    public class ApiSettings
	{
		/// <summary>
		/// Валюта по умолчанию
		/// </summary>
		public string DefaultCurrency { get; set; }

        /// <summary>
        /// Базовая валюта
        /// </summary>
        public string BaseCurrency { get; set; }

        /// <summary>
        /// Лимит запросов
        /// </summary>
        public int RequestLimit { get; set; }

        /// <summary>
        /// Использовано запросов
        /// </summary>
        public int RequestCount { get; set; }

		/// <summary>
		/// Количество знаков после запятой у курса
		/// </summary>
		public int CurrencyRoundCount { get; set; }
    }
}
