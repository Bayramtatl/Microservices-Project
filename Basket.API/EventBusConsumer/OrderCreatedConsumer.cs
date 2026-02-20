using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;

namespace Basket.API.EventBusConsumer
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IBasketRepository _repository;

        public OrderCreatedConsumer(IBasketRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            // Mesaj geldi! Sipariş veritabanına yazılmış, artık sepeti silebiliriz.
            await _repository.DeleteBasket(context.Message.UserName);
        }
    }
}