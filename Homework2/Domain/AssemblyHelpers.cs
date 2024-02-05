using System.Reflection;

namespace Fuse8_ByteMinds.SummerSchool.Domain;

public static class AssemblyHelpers
{
	/// <summary>
	/// Получает информацию о базовых типах классов из namespace "Fuse8_ByteMinds.SummerSchool.Domain", у которых есть наследники.
	/// </summary>
	/// <remarks>
	///	Информация возвращается только по самым базовым классам.
	/// Информация о промежуточных базовых классах не возвращается
	/// </remarks>
	/// <returns>Список типов с количеством наследников</returns>
	public static (string BaseTypeName, int InheritorCount)[] GetTypesWithInheritors()
	{
		// Необходимое пространство имен для поиска
		var necessaryNamespace = "Fuse8_ByteMinds.SummerSchool.Domain";

		var baseTypeList = new List<Type>();

		// Получаем все классы из текущей Assembly
		var assemblyClassTypes = Assembly.GetAssembly(typeof(AssemblyHelpers))
			!.DefinedTypes
			.Where(p => p.IsClass && !p.IsAbstract && p.Namespace == necessaryNamespace);

		foreach (var classType in assemblyClassTypes)
		{
			var baseType = GetBaseType(classType);
			if (baseType == null || baseType.Namespace != necessaryNamespace)
				continue;

			baseTypeList.Add(baseType);
		}

		return baseTypeList
			.GroupBy(p => p.Name)
			.Select(p => (p.Key, p.Count()))
			.ToArray();
	}

	/// <summary>
	/// Получает базовый тип для класса
	/// </summary>
	/// <param name="type">Тип, для которого необходимо получить базовый тип</param>
	/// <returns>
	/// Первый тип в цепочке наследований. Если наследования нет, возвращает null
	/// </returns>
	/// <example>
	/// Класс A, наследуется от B, B наследуется от C
	/// При вызове GetBaseType(typeof(A)) вернется C
	/// При вызове GetBaseType(typeof(B)) вернется C
	/// При вызове GetBaseType(typeof(C)) вернется C
	/// </example>
	private static Type? GetBaseType(Type type)
	{
		var baseType = type;

		while (baseType.BaseType is not null && baseType.BaseType != typeof(object))
		{
			baseType = baseType.BaseType;
		}

		return baseType == type
			? null
			: baseType;
	}
}