using ProjectLab.Data.Repositories.Interface;
using ProjectLab.Models.Entities;

namespace ProjectLab.Data.Repositories.Implementation
{
    public class CartProductItemRepository : RepositoryBase<CartProductItem>, ICartProductItemRepository
    {
        public CartProductItemRepository(ApplicationDBContext context)
           : base(context)
        {
        }
    }
}
