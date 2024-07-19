using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace InternalApi.Application.Interceptors;

/// <summary>
/// Interceptor для gRPC
/// </summary>
public class ServerLoggerInterceptor : Interceptor
{
	private readonly ILogger _logger;

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="loggerFactory">Сервис логирования</param>
	public ServerLoggerInterceptor(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ServerLoggerInterceptor>();
	}

	/// <summary>
	/// Обрабатывает унарные запросы
	/// </summary>
	/// <typeparam name="TRequest"></typeparam>
	/// <typeparam name="TResponse"></typeparam>
	/// <param name="request"></param>
	/// <param name="context"></param>
	/// <param name="continuation"></param>
	/// <returns></returns>
	public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
		TRequest request,
		ServerCallContext context,
		UnaryServerMethod<TRequest, TResponse> continuation)
	{
		_logger.LogInformation("Starting receiving call. Type/Method: {Type} / {Method}",
			MethodType.Unary, context.Method);
		try
		{
			return await continuation(request, context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error thrown by {context.Method}.");
			throw;
		}
	}
}
