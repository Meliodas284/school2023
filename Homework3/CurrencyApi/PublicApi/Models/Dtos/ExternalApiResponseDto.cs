namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Dtos
{
    /// <summary>
    /// Класс представляющий ответ внешнего api
    /// </summary>
    public class ExternalApiResponseDto
    {
        /// <summary>
        /// Данные ответа
        /// </summary>
        public Dictionary<string, Currency> Data { get; set; }
    }
}
