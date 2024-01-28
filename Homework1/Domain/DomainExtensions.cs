using System.Data;

namespace Fuse8_ByteMinds.SummerSchool.Domain;

public static class DomainExtensions
{
	// ToDo реализовать экстеншены

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
}