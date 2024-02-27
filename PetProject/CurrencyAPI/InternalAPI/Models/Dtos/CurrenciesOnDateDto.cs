using InternalApi.Models;

namespace InternalAPI.Models.Dtos
{
	// TODO: Комментарии к классу
	public class CurrenciesOnDateDto
	{
        public DateTime LastUpdateAt { get; set; }

        public Currency[] Currencies { get; set; }
    }
}
