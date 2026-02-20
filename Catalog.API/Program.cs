using Catalog.API.Data;
using Catalog.API.Repositories;
using Catalog.API.Services;
using Catalog.API.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    // HTTP/1.1 Kulvarý: Sadece Swagger ve REST için (Port: 8080)
    options.ListenAnyIP(8080, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });

    // HTTP/2 Kulvarý: Sadece gRPC için (Port: 8082)
    options.ListenAnyIP(8082, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

// --- 1. AYARLARI SÝSTEME TANITMA (Configuration) ---
// appsettings.json içindeki "DatabaseSettings" bölümünü DatabaseSettings sýnýfýyla eþleþtiriyoruz.
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

// Arayüz (Interface) üzerinden ayarlara her yerden eriþilmesini saðlýyoruz.
builder.Services.AddSingleton<IDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
builder.Services.AddGrpc(); // gRPC'yi kaydet

// --- 2. MONGODB CLIENT BAÐLANTISI (Singleton) ---
// Daha önce konuþtuðumuz "Connection Pooling" (Baðlantý Havuzu) avantajý için Singleton yapýyoruz.
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IDatabaseSettings>();
    return new MongoClient(settings.ConnectionString);
});


// --- 3. REPOSITORY KAYDI (Dependency Injection) ---
// Her bir HTTP isteði için bir Repository örneði oluþturulur (Scoped).
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICatalogContext, CatalogContext>();


// --- 4. STANDART API SERVÝSLERÝ ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Serilog'u Yapýlandýr
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .Enrich.WithProperty("ApplicationName", builder.Environment.ApplicationName) // Hangi servisten geldiðini ayýrmak için
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200")) //Docker içindeki adý
    {
        AutoRegisterTemplate = true,
        IndexFormat = "microservices-logs-{0:yyyy.MM.dd}",
        NumberOfReplicas = 1,
        NumberOfShards = 2
    })
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// --- 5. HTTP PIPELINE (Middleware) AYARLARI ---
// Geliþtirme modu kontrolünü kaldýrýp her koþulda çalýþmasýný saðlýyoruz
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1");
    c.RoutePrefix = "swagger"; // localhost:8080/swagger adresini garantiler
});
app.MapGrpcService<ProductService>(); // Servisi dýþarý aç
app.UseAuthorization();

app.MapControllers();

app.Run();
