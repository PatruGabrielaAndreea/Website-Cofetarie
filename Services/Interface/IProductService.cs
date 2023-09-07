using ProjectLab.Models.View;
using ProjectLab.Models.Entities;

namespace ProjectLab.Services.Interface
{
    public interface IProductService
    {
        List<Product> GetProducts();

        Product GetProductById(int productId);

        void AddProduct(Product product);

        void DeleteProduct(int productId);

        void UpdateProduct(Product product);

        public ProductModelView GetProductModelView(int productId);

        public IEnumerable<ProductModelView> GetAllByType(int productType);


        public IEnumerable<Product> GetProductsByType(int productType);

        public IEnumerable<ProductType> GetProductTypes();

    }
}
