using InternalApi.Api.Filters;
using InternalApi.Domain.Settings;
using Microsoft.OpenApi.Models;
using InternalApi.Application.DependencyInjection;
using Serilog;
using System.Reflection;
using System.Text.Json.Serialization;
using InternalApi.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
	options.Filters.Add<GlobalExceptionFilter>();
})
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});

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

builder.Services.Configure<CurrencyOptions>(builder.Configuration
	.GetSection("CurrencyAPIOptions"));

builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapHealthChecks("/health");

app.MapGrpcService<GrpcCurrencyService>()
	.RequireHost($"*:{builder.Configuration.GetValue<int>("GrpcPort")}");

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
