using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using Ordering.API.Entities;
using Ordering.API.Persistence;

public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
{
    private readonly OrderContext _dbContext;
    private readonly IMapper _mapper;

    public BasketCheckoutConsumer(OrderContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        // 1. Gelen mesajı Order entity'sine çevir
        var orderEntity = _mapper.Map<Order>(context.Message);

        // 2. Veritabanına ekle
        _dbContext.Orders.Add(orderEntity);
        await _dbContext.SaveChangesAsync();

        // 3. BURASI YENİ: Sipariş başarıyla DB'ye yazıldı, şimdi Basket.API'ye haber veriyoruz.
        // context.Publish kullanarak yeni bir event fırlatıyoruz.
        await context.Publish(new OrderCreatedEvent
        {
            UserName = orderEntity.UserName
        });

        // Log eklemek istersen:
        // _logger.LogInformation($"OrderCreatedEvent published for user: {orderEntity.UserName}");
    }
}