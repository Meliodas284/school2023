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

	/// <summary>
	/// Перегрузка оператора сложения
	/// </summary>
	/// <param name="money1">Левый операнд</param>
	/// <param name="money2">Правый операнд</param>
	/// <returns>Новый объект с суммой двух входных денежных сумм</returns>
	public static Money operator +(Money money1, Money money2)
	{
		var totalKopeks = (money1.IsNegative ? -1 : 1) * (money1.Rubles * 100 + money1.Kopeks) +
					  (money2.IsNegative ? -1 : 1) * (money2.Rubles * 100 + money2.Kopeks);

		var isNegative = totalKopeks < 0;
		totalKopeks = Math.Abs(totalKopeks);

		var rubles = totalKopeks / 100;
		var kopeks = totalKopeks % 100;

		return new Money(isNegative, rubles, kopeks);
	}

	/// <summary>
	/// Перегрузка оператора вычитание
	/// </summary>
	/// <param name="money1"></param>
	/// <param name="money2"></param>
	/// <returns>Новый объект с разницей двух входных денежных сумм</returns>
	public static Money operator -(Money money1, Money money2)
	{
		var totalKopeks = (money1.IsNegative ? -1 : 1) * (money1.Rubles * 100 + money1.Kopeks) -
					  (money2.IsNegative ? -1 : 1) * (money2.Rubles * 100 + money2.Kopeks);

		var isNegative = totalKopeks < 0;
		totalKopeks = Math.Abs(totalKopeks);

		var rubles = totalKopeks / 100;
		var kopeks = totalKopeks % 100;

		return new Money(isNegative, rubles, kopeks);
	}
	/// <summary>
	/// Проверяет, равен ли указанный объект текущему
	/// </summary>
	/// <param name="obj">Объект для сравнения</param>
	/// <returns>true: объекты равны; false: объекты не равны</returns>
	public override bool Equals(object? obj)
	{
		if (obj is not Money other)
			return false;

		return IsNegative == other.IsNegative &&
			   Rubles == other.Rubles &&
			   Kopeks == other.Kopeks;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Rubles, Kopeks);	
	}

	public override string ToString()
	{
		return $"{(IsNegative ? "-" : "")}{Rubles}.{Kopeks}";
	}
}