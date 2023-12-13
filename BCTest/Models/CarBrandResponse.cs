namespace BCTest.Models
{
	public class CarBrandResponse
	{
		public List<CarBrand> CarBrands { get; set; } = new List<CarBrand>();
		public int TotalPages { get; set; }
	}
}
