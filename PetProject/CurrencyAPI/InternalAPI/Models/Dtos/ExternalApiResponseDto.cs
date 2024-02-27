using System.Text.Json.Serialization;

namespace InternalApi.Models.Dtos
{
    /// <summary>
    /// Класс представляющий ответ внешнего API
    /// </summary>
    public class ExternalApiResponseDto
    {
        /// <summary>
        /// Метаданные
        /// </summary>
        public MetaDto Meta { get; set; }

        /// <summary>
        /// Данные ответа
        /// </summary>
        public Dictionary<string, Currency> Data { get; set; }
    }

    /// <summary>
    /// Метаданные ответа внешнего API
    /// </summary>
    public class MetaDto
    {
        /// <summary>
        /// Дата обновления данных
        /// </summary>
        [JsonPropertyName("last_updated_at")]
        public DateTime LastUpdatedAt { get; set; }
    }
}
