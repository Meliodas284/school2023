using System.Net;

namespace Fuse8_ByteMinds.SummerSchool.Domain;

public static class ExceptionHandler
{
	/// <summary>
	/// Обрабатывает исключение, которое может возникнуть при выполнении <paramref name="action"/>
	/// </summary>
	/// <param name="action">Действие, которое может породить исключение</param>
	/// <returns>Сообщение об ошибке</returns>
	public static string? Handle(Action action)
	{
		try
		{
			action();
		}
		catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
		{
			return "Ресурс не найден";
		}
		catch (HttpRequestException ex)
		{
			return ex.StatusCode.ToString();
		}
		catch (NotValidKopekCountException ex)
		{
			return ex.Message;
		}
		catch (NegativeRubleCountException ex)
		{
			return ex.Message;
		}
		catch (MoneyException ex)
		{
			return ex.Message;
		}
		catch
		{
			return "Произошла непредвиденная ошибка";
		}

		return null;
	}
}

public class MoneyException : Exception
{
	public MoneyException()
	{
	}

	public MoneyException(string? message)
		: base(message)
	{
	}
}

public class NotValidKopekCountException : MoneyException
{
	public NotValidKopekCountException() : base("Количество копеек должно быть больше 0 и меньше 99")
	{
	}
}

public class NegativeRubleCountException : MoneyException
{
	public NegativeRubleCountException() : base("Число рублей не может быть отрицательным")
	{
	}
}