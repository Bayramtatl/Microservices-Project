using AutoMapper;
using Basket.API.Models;
using EventBus.Messages.Events;

namespace Basket.API.Mappers
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            // BasketCheckout DTO'su ile BasketCheckoutEvent (Mesaj) arasını bağla
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
