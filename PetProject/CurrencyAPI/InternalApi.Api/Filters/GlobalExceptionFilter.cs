using InternalApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InternalApi.Api.Filters;

/// <summary>
/// Представляет глобальный фильтр исключений
/// </summary>
public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    /// <summary>
	/// Конструктор для инициализации зависимостей
	/// </summary>
	/// <param name="logger">Логгер</param>
	public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Метод предназначенный для обработки исключений <see cref="IExceptionFilter"/>
    /// </summary>
    /// <param name="context">Контекст для фильтра исключений</param>
    public void OnException(ExceptionContext context)
    {
        ProblemDetails problemDetails;

        switch (context.Exception)
        {
            case CurrencyNotFoundException:
                problemDetails = new ProblemDetails
                {
                    Status = 404,
                    Title = "Валюта не найдена",
                    Detail = "Код базовой или искомой валюты, или дата курса некорректны!"
                };
                break;
            case ApiRequestLimitException:
                problemDetails = new ProblemDetails
                {
                    Status = 429,
                    Title = "Превышен лимит запросов",
                    Detail = "На вашем счету превышен лимит запросов к API"
                };
                _logger.LogError(context.Exception, "Превышен лимит запросов к API");
                break;
            default:
                problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Title = "Непредвиденная ошибка",
                    Detail = "Произошла непредвиденная ошибка"
                };
                _logger.LogError(context.Exception, "Непредвиденная ошибка");
                break;
        }

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };

        context.ExceptionHandled = true;
    }
}
