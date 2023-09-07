
using ProjectLab.Data.Repositories.Interface;
using ProjectLab.Models.Entities;

namespace ProjectLab.Data.Repositories.Implementation
{
    public class OrderStatusRepository : RepositoryBase<OrderStatus>, IOrderStatusRepository
    {
        public OrderStatusRepository(ApplicationDBContext context)
           : base(context)
        {
        }
    }
}
