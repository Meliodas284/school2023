using InternalAPI.Exceptions;
using InternalAPI.Models;
using InternalAPI.Services.CachedCurrencyAPIService;
using InternalAPI.Services.CacheFileService;
using InternalAPI.Services.CurrencyAPIService;
using InternalAPI.Services.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
	options.Filters.Add<GlobalExceptionFilter>();
})
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
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

builder.Services.AddSerilog(loggerConfig =>
	loggerConfig.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddHttpClient("currency", client =>
{
	client.DefaultRequestHeaders.Add("apikey", builder.Configuration["API-KEY"]);
	client.BaseAddress = new Uri(builder.Configuration["CurrencyAPIOptions:BaseUrl"]!);
});

builder.Services.Configure<CurrencyApiOptions>(builder.Configuration
	.GetSection("CurrencyAPIOptions"));

builder.Services.AddHealthChecks()
	.AddCheck<CurrencyHealthCheck>("custom-currency", HealthStatus.Unhealthy);

builder.Services.AddScoped<ICurrencyAPIService, CurrencyAPIService>();
builder.Services.AddScoped<ICacheFileService, CacheFileService>();
builder.Services.AddScoped<ICachedCurrencyAPIService, CachedCurrencyAPIService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
