﻿namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models
{
	/// <summary>
	/// Класс представляющий валюту
	/// </summary>
	public class Currency
	{
        /// <summary>
		/// Код валюты
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// Курс валюты
		/// </summary>
		public double Value { get; set; }
    }
}
