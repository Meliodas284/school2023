namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Dtos
{
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
        /// Код валюты
        /// </summary>
        public string Code { get; set; }

		/// <summary>
		/// Курс валюты
		/// </summary>
		public double Value { get; set; }
	}
}
