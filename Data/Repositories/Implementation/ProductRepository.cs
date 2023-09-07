
using ProjectLab.Data.Repositories.Interface;
using ProjectLab.Models.Entities;

namespace ProjectLab.Data.Repositories.Implementation
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDBContext context)
           : base(context)
        {
        }
    }
}
