using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace WebDev_Labb2_API.Model
{
    [Collection("Products")]

    public class Products
    {
        public ObjectId Id { get; set; }
        public int sku { get; set; }
        public double price { get; set; }
        public bool in_stock { get; set; }
        public string name { get; set; }
        public string? appearance { get; set; }
        public double atomic_mass { get; set; }
        public string category { get; set; }
        public double density { get; set; }
        public double? melt { get; set; }
        public double? boil { get; set; }
        public double number { get; set; }
        public string phase { get; set; }
        public string source { get; set; }
        public string? bohr_model_image { get; set; }
        public string summary { get; set; }
        public string symbol { get; set; }
        public string cpk_hex { get; set; }
        public string block { get; set; }
    }
}