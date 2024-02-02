namespace Fuse8_ByteMinds.SummerSchool.Domain;

/// <summary>
/// Значения ресурсов для календаря
/// </summary>
public class CalendarResource
{
	public static readonly CalendarResource Instance;

	public static readonly string January;
	public static readonly string February;

	private static readonly string[] MonthNames;

	static CalendarResource()
	{
		Instance = new CalendarResource();

		MonthNames = new[]
		{
			"Январь",
			"Февраль",
			"Март",
			"Апрель",
			"Май",
			"Июнь",
			"Июль",
			"Август",
			"Сентябрь",
			"Октябрь",
			"Ноябрь",
			"Декабрь",
		};

		January = GetMonthByNumber(0);
		February = GetMonthByNumber(1);
	}

	/// <summary>
	/// Возвращает название месяца по его номеру
	/// </summary>
	/// <param name="number">Номер месяца</param>
	/// <returns>Название месяца по номеру</returns>
	/// <exception cref="ArgumentOutOfRangeException">Если переданный номер отрицательный 
	/// или больше 12</exception>
	private static string GetMonthByNumber(int number)
	{
		if (number < 0 || number > 12)
			throw new ArgumentOutOfRangeException("Номер месяца должен быть от 0 до 12");

		return MonthNames[number];
	}

	/// <summary>
	/// Индексатор для получения названия месяца по <see cref="Month"/>
	/// </summary>
	/// <param name="month">Индекс</param>
	/// <returns>Название месяца по индексу <see cref="Month"/></returns>
	public string this[Month month] => GetMonthByNumber((int)month);
}

public enum Month
{
	January,
	February,
	March,
	April,
	May,
	June,
	July,
	August,
	September,
	October,
	November,
	December,
}