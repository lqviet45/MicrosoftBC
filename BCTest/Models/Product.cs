using System.Numerics;

namespace BCTest.Models
{
    public class Product
    {
        public long Id { get; set; }
        public required string ProductName { get; set; }
        public required string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public string? Barcode { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
