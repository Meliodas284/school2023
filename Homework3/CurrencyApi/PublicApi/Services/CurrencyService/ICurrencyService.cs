using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Dtos;

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

		/// <summary>
		/// Получить курс валюты по коду и дате
		/// </summary>
		/// <param name="date">Дата курса</param>
		/// <param name="code">Код валюты</param>
		/// <returns>Информацию о валюте с нужным кодом
		/// и на определенную дату<see cref="Currency"/></returns>
		Task<DateCurrencyDto> GetCurrencyOnDate(DateOnly date, string code);

		/// <summary>
		/// Получить настройки API
		/// </summary>
		/// <returns>Информацию об API</returns>
		Task<ApiSettings> GetSettings();
	}
}
