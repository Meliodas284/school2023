﻿using InternalApi.Models;
using InternalAPI.Models.Dtos;

namespace InternalAPI.Services.CacheFileService;

/// <summary>
/// Предоставляет методы работы с файлами кэша
/// </summary>
public interface ICacheFileService
{
	/// <summary>
	/// Получить курс валюты по заданному типу из самого раннего файла кэша
	/// Если файла нет, получить данные по API и сохранить в кэш
	/// </summary>
	/// <param name="type">Тип валюты (код)</param>
	/// <param name="token">Токен отмены</param>
	/// <returns>Курс валюты заданного типа</returns>
	Task<Currency> GetCurrency(CurrencyType type, CancellationToken token);

	/// <summary>
	/// Получить курс валюты по заданному типу на нужную дату из кэша
	/// Если файла нет, получить данные по API и сохранить в кэш
	/// </summary>
	/// <param name="type">Тип валюты (код)</param>
	/// <param name="date">Нужная дата</param>
	/// <param name="token">Токен отмены</param>
	/// <returns>Курс валюты заданного типа на нужную дату</returns>
	Task<Currency> GetCurrencyOnDate(CurrencyType type, DateOnly date, CancellationToken token);
}
