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
        public double? boil { get; set; }
        public string category { get; set; }
        public double? density { get; set; }
        public string? discovered_by { get; set; }
        public double? melt { get; set; }
        public double? molar_heat { get; set; }
        public string? named_by { get; set; }
        public int number { get; set; }
        public int? period { get; set; }
        public int? group { get; set; }
        public string phase { get; set; }
        public string? source { get; set; }
        public string? bohr_model_image { get; set; }
        public string? bohr_model_3d { get; set; }
        public string? spectral_img { get; set; }
        public string summary { get; set; }
        public string symbol { get; set; }
        public int? xpos { get; set; }
        public int? ypos { get; set; }
        public int? wxpos { get; set; }
        public int? wypos { get; set; }
        public int[]? shells { get; set; }
        public string? electron_configuration { get; set; }
        public string? electron_configuration_semantic { get; set; }
        public double? electron_affinity { get; set; }
        public double? electronegativity_pauling { get; set; }
        public double[]? ionization_energies { get; set; }
        public string cpk_hex { get; set; }
        public string block { get; set; }
    }
}