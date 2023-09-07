using ProjectLab.Data.Repositories.Interface;
using ProjectLab.Models.Entities;

namespace ProjectLab.Data.Repositories.Implementation
{
    public class ProductTypeRepository : RepositoryBase<ProductType>, IProductTypeRepository
    {
        public ProductTypeRepository(ApplicationDBContext context)
           : base(context)
        {
        }
    }
}
