using Audit.Core;
using Audit.Http;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Exceptions;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Services.CurrencyService;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi;

/// <summary>
/// Класс настраивает конфигурацию приложения
/// </summary>
public class Startup
{
	private readonly IConfiguration _configuration;

	/// <summary>
	/// Конструктор инициализирует внедренные зависимости
	/// </summary>
	/// <param name="configuration">Сервис конфигурации</param>
	public Startup(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	/// <summary>
	/// Добавляет сервисы приложения
	/// </summary>
	/// <param name="services">Коллекция сервисов</param>
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddControllers(options =>
		{
			options.Filters.Add<GlobalExceptionFilter>();
		})

			// Добавляем глобальные настройки для преобразования Json
			.AddJsonOptions(
				options =>
				{
					// Добавляем конвертер для енама
					// По умолчанию енам преобразуется в цифровое значение
					// Этим конвертером задаем перевод в строковое значение
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				});

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo
			{
				Version = "v1",
				Title = "Currency API",
				Description = "An ASP.NET Core Web API for currency",
			});

			var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
		});
		
		services.AddSerilog(loggerConfig =>
			loggerConfig.ReadFrom.Configuration(_configuration));

		Audit.Core.Configuration.Setup()
			.UseSerilog();

		services.AddHttpClient("currency", client =>
		{
			client.DefaultRequestHeaders.Add("apikey", _configuration["API-KEY"]);
			client.BaseAddress = new Uri(_configuration["CurrencyAPIOptions:BaseUrl"]!);
		})
		.AddAuditHandler(audit => audit
			.IncludeRequestBody()
			.IncludeRequestHeaders()
			.IncludeContentHeaders()
			.IncludeResponseHeaders()
			.IncludeResponseBody());

		services.Configure<CurrencyAPIOptions>(_configuration.GetSection("CurrencyAPIOptions"));

		services.AddScoped<ICurrencyService, CurrencyService>();
	}

	/// <summary>
	/// Конфигурация middleware компонентов
	/// </summary>
	/// <param name="app">Builder приложения для конфигурации</param>
	/// <param name="env">Информация о среде веб-хостинга</param>
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseRouting();

		app.UseSerilogRequestLogging();

		app.UseEndpoints(endpoints => endpoints.MapControllers());
	}
}