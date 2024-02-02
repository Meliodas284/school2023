namespace Fuse8_ByteMinds.SummerSchool.Domain;

public static class DomainExtensions
{
	/// <summary>
	/// Сравнивает два объекта Money
	/// </summary>
	/// <param name="money1"></param>
	/// <param name="money2"></param>
	/// <returns>-1: левый меньше правого; 0: равны, 1: левый больше правого</returns>
	public static int CompareTo(this Money money1, Money money2)
	{
		var thisAmount = money1.Rubles * 100 + money1.Kopeks;
		var otherAmount = money2.Rubles * 100 + money2.Kopeks;

		if (money1.IsNegative != money2.IsNegative)
			return money1.IsNegative ? -1 : 1;

		return !money1.IsNegative ? thisAmount.CompareTo(otherAmount) : 
			-thisAmount.CompareTo(otherAmount);
	}

	/// <summary>
	/// Определяет пустая ли коллекция или равна ли она null
	/// </summary>
	/// <param name="mass">Коллекция</param>
	/// <returns>true: коллекция пуста или равно null; false: иначе</returns>
	public static bool IsNullOrEmpty<T>(this IEnumerable<T>? mass) => mass is null || !mass.Any();

	/// <summary>
	/// Объединяет элементы коллекции в строку по разделителю
	/// </summary>
	/// <param name="mass">Коллекция</param>
	/// <param name="delimiter">Разделитель</param>
	/// <returns>Объединенную строку</returns>
	public static string JoinToString<T>(this IEnumerable<T> mass, string delimiter) => 
		string.Join(delimiter, mass);

	/// <summary>
	/// Определяет количество дней между двумя датами
	/// </summary>
	/// <param name="firstDate">Первая дата</param>
	/// <param name="secondDate">Вторая дата</param>
	/// <returns>Количество дней между датами</returns>
	public static int DaysCountBetween(this DateTimeOffset firstDate, DateTimeOffset secondDate) =>
		(int)(firstDate - secondDate).TotalDays;
}