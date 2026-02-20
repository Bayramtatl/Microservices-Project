using AutoMapper;
using Basket.API.Entities;
using Basket.API.Models;
using Basket.API.Repositories;
using Catalog.API.Protos;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly ProductProtoService.ProductProtoServiceClient _productServiceClient; //gRPC
        private readonly IMapper _mapper; // AutoMapper için
        private readonly IPublishEndpoint _publishEndpoint; // MassTransit (RabbitMQ) için

        public BasketController(
            IBasketRepository repository,
            ProductProtoService.ProductProtoServiceClient productServiceClient,
            IMapper mapper,
            IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _productServiceClient = productServiceClient;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);

            // Eğer sepet boşsa, kullanıcıya boş bir sepet nesnesi dönüyoruz (Yeni sepet başlatmak gibi)
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            foreach (var item in basket.Items)
            {
                // Catalog API'den ürün bilgilerini çekiyoruz
                var product = await _productServiceClient.GetProductAsync(new GetProductRequest { Id = item.ProductId });

                // Ürün ismini ve fiyatını Catalog'dan gelen gerçek verilerle güncelliyoruz
                item.ProductName = product.Name;
                item.Price = (decimal)product.Price;
            }

            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }

        [HttpPost("Checkout")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)] // 202 için
        [ProducesResponseType((int)HttpStatusCode.BadRequest)] // 400 için
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // 1. Sepeti getir
            var basket = await _repository.GetBasket(basketCheckout.UserName);
            if (basket == null) return BadRequest();

            // 2. Event oluştur ve TotalPrice'ı hesapla
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice; // Toplam fiyatı Redis'teki güncel veriden alıyoruz

            // 3. RabbitMQ'ya gönder
            await _publishEndpoint.Publish(eventMessage);


            return Accepted();
        }
    }
}