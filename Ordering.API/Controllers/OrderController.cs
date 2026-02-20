using Microsoft.AspNetCore.Mvc;
using Ordering.API.Entities;
using Ordering.API.Repository;
using System.Net;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _repository;

        public OrderController(IOrderRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{userName}", Name = "GetOrder")]
        [ProducesResponseType(typeof(IEnumerable<Order>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUserName(string userName)
        {
            var orders = await _repository.GetOrdersByUserName(userName);
            return Ok(orders);
        }
    }
}
