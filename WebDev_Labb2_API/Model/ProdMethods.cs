namespace WebDev_Labb2_API.Model
{
    public class ProdMethods
    {
        public Products AssignProductValues(Products oldProduct, Products newProduct)
        {
            oldProduct.price = newProduct.price;
            oldProduct.in_stock = newProduct.in_stock;
            oldProduct.name = newProduct.name;
            oldProduct.appearance = newProduct.appearance;
            oldProduct.atomic_mass = newProduct.atomic_mass;
            oldProduct.category = newProduct.category;
            oldProduct.density = newProduct.density;
            oldProduct.melt = newProduct.melt;
            oldProduct.boil = newProduct.boil;
            oldProduct.number = newProduct.number;
            oldProduct.phase = newProduct.phase;
            oldProduct.source = newProduct.source;
            oldProduct.bohr_model_image = newProduct.bohr_model_image;
            oldProduct.summary = newProduct.summary;
            oldProduct.symbol = newProduct.symbol;
            oldProduct.cpk_hex = newProduct.cpk_hex;
            oldProduct.block = newProduct.block;
            return oldProduct;
        }
    }
}
