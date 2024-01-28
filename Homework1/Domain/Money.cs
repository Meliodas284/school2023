namespace Fuse8_ByteMinds.SummerSchool.Domain;

/// <summary>
/// Модель для хранения денег
/// </summary>
public class Money
{
	public Money(int rubles, int kopeks)
		: this(false, rubles, kopeks)
	{
	}

	public Money(bool isNegative, int rubles, int kopeks)
	{
		if (kopeks is < 0 or > 99) 
			throw new ArgumentException("Количество копеек не должно быть отрицательным или больше 99");

		if (rubles < 0) 
			throw new ArgumentException("Количество рублей не может быть отрицательным");

		if (isNegative && rubles == 0 && kopeks == 0)
			throw new ArgumentException("Денежная сумма не может быть отрицательной и равна нулю");

		IsNegative = isNegative;
		Rubles = rubles;
		Kopeks = kopeks;
	}

	/// <summary>
	/// Отрицательное значение
	/// </summary>
	public bool IsNegative { get; }

	/// <summary>
	/// Число рублей
	/// </summary>
	public int Rubles { get; }

	/// <summary>
	/// Количество копеек
	/// </summary>
	public int Kopeks { get; }
}