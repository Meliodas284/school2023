using InternalApi.Domain.Exceptions;
using InternalApi.Domain.Interfaces.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace InternalApi.Application.HealthCheck;

/// <summary>
/// Реализует HealthCheck для проверки доступности API
/// </summary>
public class CurrencyHealthCheck : IHealthCheck
{
	private readonly ICurrencyApiService _apiService;

        /// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="apiService">Сервис API</param>
	public CurrencyHealthCheck(ICurrencyApiService apiService)
        {
		_apiService = apiService;
	}

	/// <summary>
	/// Метод для проверки доступности API
	/// </summary>
	/// <param name="context">Контекст</param>
	/// <param name="token">Токен отмены</param>
	/// <returns>api доступно: Healthy; api не доступно: Unhealthy</returns>
	/// <exception cref="ApiRequestLimitException">При превышении лимита запросов к API</exception>
	public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken token = default)
	{
		try
		{
			var settings = await _apiService.GetApiSettingsAsync(token);
			if (settings.RequestCount >= settings.RequestLimit)
				throw new ApiRequestLimitException("Превышен лимит запросов к API");

			return HealthCheckResult.Healthy();
		}
		catch (Exception ex) 
		{ 
			return HealthCheckResult.Unhealthy(ex.Message);
		}
	}
}
