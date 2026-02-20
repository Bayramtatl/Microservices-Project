using AutoMapper;
using EventBus.Messages.Events;
using Ordering.API.Entities;
using AutoMapper;

namespace Ordering.API.Mappings
{
    public class OrderProfile :Profile
    {
        public OrderProfile()
        {
            // EventBus.Messages'tan gelen Event'i, kendi Entity'mize çeviriyoruz
            CreateMap<BasketCheckoutEvent, Order>()
    .ForMember(dest => dest.AddressLine, opt => opt.MapFrom(src => src.AddressLine)) // Eğer isimler farklıysa burayı düzenle
    .ReverseMap();
        }
    }
}
