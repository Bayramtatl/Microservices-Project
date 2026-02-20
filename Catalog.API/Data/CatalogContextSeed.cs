using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            // Veritabanında ürün var mı kontrol et
            bool existProduct = productCollection.Find(p => true).Any();

            if (!existProduct)
            {
                // Eğer yoksa, örnek verileri listeye ekle
                productCollection.InsertManyAsync(GetPreconfiguredProducts());
            }
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "IPhone 15",
                    Summary = "Apple'ın en yeni amiral gemisi.",
                    Description = "Harika kamera özellikleri ve hızlı işlemci.",
                    ImageFile = "product-1.png",
                    Price = 55000,
                    Category = "Smart Phone"
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f6",
                    Name = "Samsung S24",
                    Summary = "Yapay zeka destekli akıllı telefon.",
                    Description = "Üstün ekran kalitesi ve S-Pen desteği.",
                    ImageFile = "product-2.png",
                    Price = 48000,
                    Category = "Smart Phone"
                }
            };
        }
    }
}
