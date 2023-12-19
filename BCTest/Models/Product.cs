using System.Numerics;

namespace BCTest.Models
{
    public class Product
    {
        public long ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal Price { get; set; }
        public string? Barcode { get; set; }
        public DateTime CreateDate { get; set; }
        //public int Stock { get; set; }
    }
}
