namespace InternalApi.Exceptions
{
	/// <summary>
	/// Исключение при неизвестном коде валюты
	/// </summary>
	public class CurrencyNotFoundException : Exception
	{
		/// <summary>
		/// Конструктор по умолчанию
		/// </summary>
		public CurrencyNotFoundException() { }

		/// <summary>
		/// Конструктор с сообщением
		/// </summary>
		/// <param name="message">Сообщение ошибки</param>
		public CurrencyNotFoundException(string message)
            : base(message) { }
    }
}
