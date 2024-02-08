namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Dtos
{
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
