﻿namespace Fuse8_ByteMinds.SummerSchool.Domain;

/// <summary>
/// Контейнер для значения, с отложенным получением
/// </summary>
public class Lazy<TValue>
{
	private readonly System.Lazy<TValue> _lazy;

	public Lazy(Func<TValue> func)
    {
        _lazy = new System.Lazy<TValue>(func);
    }

	public TValue? Value => _lazy.Value;
}