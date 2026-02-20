using Ordering.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.API.Repository
{
    public interface IOrderRepository
    {
        // Sadece kullanıcı adına göre siparişleri getiren metot
        Task<IEnumerable<Order>> GetOrdersByUserName(string userName);

        // Eğer başka metotlara ihtiyacın olursa buraya ekleyebilirsin
    }
}
