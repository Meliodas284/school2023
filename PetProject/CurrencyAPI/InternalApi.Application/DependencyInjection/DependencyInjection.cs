using InternalApi.Application.HealthCheck;
using InternalApi.Application.Interceptors;
using InternalApi.Application.Services;
using InternalApi.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace InternalApi.Application.DependencyInjection;

public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services)
	{
		services.AddGrpc(options =>
		{
			options.Interceptors.Add<ServerLoggerInterceptor>();
		});

		services.AddHealthChecks()
			.AddCheck<CurrencyHealthCheck>("custom-currency", HealthStatus.Unhealthy);

		InitServices(services);
	}

	private static void InitServices(IServiceCollection services)
	{
		services.AddScoped<ICurrencyApiService, CurrencyApiService>();
		services.AddScoped<ICacheFileService, CacheFileService>();
		services.AddScoped<ICacheCurrencyService, CacheCurrencyService>();
	}
}
