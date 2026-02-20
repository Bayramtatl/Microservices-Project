using Basket.API.EventBusConsumer;
using Basket.API.Repositories;
using Catalog.API.Protos;
using MassTransit;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Redis Yapýlandýrmasý
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
builder.Services.AddGrpcClient<ProductProtoService.ProductProtoServiceClient>(options =>
{
    // Docker üzerinde catalogapi ismindeki servise, gRPC portundan (80) baðlanacaðýz
    options.Address = new Uri(builder.Configuration.GetValue<string>("GrpcSettings:CatalogUrl"));
});
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBasketRepository, BasketRepository>(); // Redis kaydý
builder.Services.AddAutoMapper(typeof(Program));
// MassTransit Yapýlandýrmasý
builder.Services.AddMassTransit(config => {

    // 1. ADIM: Yeni Consumer'ý MassTransit'e tanýtýyoruz
    config.AddConsumer<OrderCreatedConsumer>();

    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

        // 2. ADIM: Bu mikroservisin hangi kuyruðu dinleyeceðini belirtiyoruz
        cfg.ReceiveEndpoint("order-created-queue", c => {
            // Gelen "OrderCreatedEvent" mesajlarýný bu consumer iþlesin diyoruz
            c.ConfigureConsumer<OrderCreatedConsumer>(ctx);
        });
    });
});

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
