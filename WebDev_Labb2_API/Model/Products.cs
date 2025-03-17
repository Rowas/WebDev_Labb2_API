using MongoDB.EntityFrameworkCore;

namespace WebDev_Labb2_API
{
    [Collection("Products")]
    public class Products
    {
        public string SKU { get; set; }
        public double Price { get; set; }
        public bool IsInStock { get; set; }
        public string Name { get; set; }
        public string Apperance { get; set; }
        public double AtomicMass { get; set; }
        public string Category { get; set; }
        public double Density { get; set; }
        public double MeltingPoint { get; set; }
        public double BoilingPoint { get; set; }
        public double AtomicNumber { get; set; }
        public string Phase { get; set; }
        public string Source { get; set; }
        public string BohrModelImg { get; set; }
        public string Summary { get; set; }
        public string Symbol { get; set; }
        public string CpkHexColor { get; set; }
        public string Block { get; set; }
    }
}