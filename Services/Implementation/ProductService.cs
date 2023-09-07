using ProjectLab.Data.Repositories.Interface;
using ProjectLab.Models.Entities;
using ProjectLab.Models.View;
using ProjectLab.Services.Interface;

namespace ProjectLab.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        private readonly IProductTypeRepository _productTypeRepository;

        public ProductService(IProductRepository productRepository, IProductTypeRepository productTypeRepository)
        {
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
        }

        public void AddProduct(Product product)
        {
            _productRepository.Create(product);
            _productRepository.Save();
        }

        public void DeleteProduct(int productId)
        {
            _productRepository.Delete(_productRepository.FindByCondition(p => p.Id == productId).ToList().ElementAt(0));
            _productRepository.Save();
        }

        public Product GetProductById(int productId)
        {
            return _productRepository.FindByCondition(p => p.Id == productId).ToList().ElementAt(0);
        }

        public List<Product> GetProducts()
        {
            return _productRepository.FindByCondition(p => !p.isDeleted).ToList();
        }

        public void UpdateProduct(Product product)
        {
            _productRepository.Update(product);
            _productRepository.Save();
        }
        public ProductModelView GetProductModelView(int productId)
        {
            Product product = GetProductById(productId);
            var productModelView = new ProductModelView
            {
                ProductId = product.Id,
                Description = product.Description,
                Name = product.Name,
                Price = product.Price,
                FilePath = product.FilePath,
                Photo = product.Photo
            };

            return productModelView;
        }

        public IEnumerable<ProductModelView> GetAllByType(int productType)
        {
            var products = _productRepository.FindByCondition(product => product.Category.Id == productType && !product.isDeleted);

            foreach (var product in products)
            {
                yield return GetProductModelView(product.Id);
            }
        }

        public IEnumerable<ProductType> GetProductTypes()
        {
            return _productTypeRepository.FindAll();
        }

        public IEnumerable<Product> GetProductsByType(int productType)
        {
            return _productRepository.FindByCondition(product => product.Category.Id == productType && !product.isDeleted);
        }
    }

}
