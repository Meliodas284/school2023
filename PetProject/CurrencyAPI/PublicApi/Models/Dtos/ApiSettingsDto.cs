namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Dtos
{
    /// <summary>
    /// Представляет информацию о настройках API
    /// </summary>
    public class ApiSettingsDto
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
		/// Есть ли еще доступные запросы
		/// </summary>
		public bool NewRequestAvailable { get; set; }

        /// <summary>
        /// Количество знаков после запятой у курса
        /// </summary>
        public int CurrencyRoundCount { get; set; }
    }
}
