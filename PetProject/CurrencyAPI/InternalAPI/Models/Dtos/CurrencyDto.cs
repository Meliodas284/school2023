namespace InternalAPI.Models.Dtos
{
	public class CurrencyDto
	{
		public CurrencyType CurrencyType { get; set; }

        public double Value { get; set; }
    }

	public enum CurrencyType
	{
		USD,
		RUB,
		KZT
	}
}
