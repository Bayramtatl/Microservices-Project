using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext:ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            // appsettings.json veya docker-compose'dan bağlantı bilgilerini alıyoruz
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));

            // Az önce konuştuğumuz Seeding (Örnek veri ekleme) işlemini burada tetikliyoruz
            CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }
    }
}
