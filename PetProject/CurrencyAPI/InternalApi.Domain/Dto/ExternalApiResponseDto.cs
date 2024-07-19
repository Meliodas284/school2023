using InternalApi.Domain.Entity;
using System.Text.Json.Serialization;

namespace InternalApi.Domain.Dto;

/// <summary>
/// Класс представляющий ответ внешнего API
/// </summary>
public class ExternalApiResponseDto
{
    /// <summary>
    /// Метаданные
    /// </summary>
    [JsonPropertyName("meta")]
	public MetaDto Meta { get; set; }

    /// <summary>
    /// Данные ответа
    /// </summary>
    [JsonPropertyName("data")]
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
