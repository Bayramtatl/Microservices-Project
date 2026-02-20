using Microsoft.EntityFrameworkCore;
using Ordering.API.Entities;
using Ordering.API.Persistence;
using Ordering.API.Repository;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _dbContext;

        public OrderRepository(OrderContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            return await _dbContext.Orders
                                .Where(o => o.UserName == userName)
                                .ToListAsync();
        }
    }
}