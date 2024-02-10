using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services.CurrencyService
{
	/// <summary>
	/// Предоставляет методы для получения курса валют
	/// </summary>
	public interface ICurrencyService
	{
		/// <summary>
		/// Получить курс валюты по умолчанию
		/// </summary>
		/// <returns>Информация о валюте <see cref="Currency"/></returns>
		Task<Currency> GetCurrency();

		/// <summary>
		/// Получить курс валюты по коду
		/// </summary>
		/// <param name="code">Код валюты</param>
		/// <returns>Информацию о валюте с нужным кодом <see cref="Currency"/></returns>
		Task<Currency> GetCurrencyByCode(string code);
	}
}
