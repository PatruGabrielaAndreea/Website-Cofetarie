using ProjectLab.Data.Repositories.Interface;
using ProjectLab.Models.Entities;

namespace ProjectLab.Data.Repositories.Implementation
{
    public class OrderProductItemRepository : RepositoryBase<OrderProductItem>, IOrderProductItemRepository
    {
        public OrderProductItemRepository(ApplicationDBContext context)
           : base(context)
        {
        }
    }
}
