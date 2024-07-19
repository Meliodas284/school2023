using Microsoft.Extensions.DependencyInjection;
using PublicApi.Application.Services;
using PublicApi.Domain.Interfaces.Services;

namespace PublicApi.Application.DependencyInjection;

public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services)
	{
		InitServices(services);
	}

	private static void InitServices(IServiceCollection services)
	{
		services.AddScoped<ICurrencyService, CurrencyService>();
	}
}
