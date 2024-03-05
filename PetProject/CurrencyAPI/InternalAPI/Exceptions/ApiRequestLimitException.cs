namespace InternalAPI.Exceptions;

/// <summary>
/// Исключение при истечении запросов по API
/// </summary>
public class ApiRequestLimitException : Exception
{
	/// <summary>
	/// Конструктор по умолчанию
	/// </summary>
	public ApiRequestLimitException() { }

	/// <summary>
	/// Конструктор с сообщением
	/// </summary>
	/// <param name="message">Сообщение ошибки</param>
	public ApiRequestLimitException(string message) 
            : base(message) { }
}
