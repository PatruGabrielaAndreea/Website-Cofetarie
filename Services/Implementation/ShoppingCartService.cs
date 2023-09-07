using ProjectLab.Data.Repositories.Interface;
using ProjectLab.Models.Entities;
using ProjectLab.Models.View;
using ProjectLab.Models.Views;
using ProjectLab.Services.Interface;

namespace ProjectLab.Services.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ICartProductItemRepository _shoppingCartItemRepository;

        private readonly IProductRepository _productRepository;

        private readonly IOrderProductItemRepository _orderProductItemRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IUserService _userService;


        public ShoppingCartService(
            ICartProductItemRepository shoppingCartItemRepository,
            IProductRepository productRepository,
            IOrderProductItemRepository orderProductItemRepository,
            IOrderRepository orderRepository,
            IUserService userService)
        {
            _productRepository = productRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _orderProductItemRepository = orderProductItemRepository;
            _orderRepository = orderRepository;
            _userService = userService;
        }

        public void AddProductInCart(string userId, int productId)
        {
            List<CartProductItem> products = _shoppingCartItemRepository.FindByCondition(p => p.ProductId == productId && p.UserId == userId).ToList();
            if (products != null && products.Count > 0)
            {
                EditProductCartQuantity(userId, productId, products.ElementAt(0).Quantity + 1);
            }
            else
            {
                CartProductItem cartProductItem = new CartProductItem()
                {
                    ProductId = productId,
                    UserId = userId,
                    Quantity = 1,
                };
                _shoppingCartItemRepository.Create(cartProductItem);
                _shoppingCartItemRepository.Save();
            }

        }

        public void EditProductCartQuantity(string userId, int productId, int newQuantity)
        {
            CartProductItem? product = _shoppingCartItemRepository.FindByCondition(p => p.ProductId == productId && p.UserId == userId).ToList().FirstOrDefault();
            if (product != null)
            {
                product.Quantity = newQuantity;
                _shoppingCartItemRepository.Update(product);
                _shoppingCartItemRepository.Save();
            }
        }

        public void increaseProductQuantity(string userId, int productId)
        {
            CartProductItem product = _shoppingCartItemRepository.FindByCondition(p => p.ProductId == productId && p.UserId == userId).ToList().ElementAt(0);
            EditProductCartQuantity(userId, productId, product.Quantity + 1);
        }

        public void decreaseProductQuantity(string userId, int productId)
        {
            CartProductItem product = _shoppingCartItemRepository.FindByCondition(p => p.ProductId == productId && p.UserId == userId).ToList().ElementAt(0);
            EditProductCartQuantity(userId, productId, product.Quantity == 0 ? 0 : product.Quantity - 1);
        }

        public List<Product> getUserShoppingCartProducts(string userId)
        {
            List<CartProductItem> productsItems = _shoppingCartItemRepository.FindByCondition(p => p.UserId == userId).ToList();
            List<Product> products = new List<Product>();

            foreach (CartProductItem product in productsItems)
            {
                products.Add(_productRepository.FindByCondition(p => p.Id == product.ProductId).ToList()
                    .ElementAt(0));
            }

            return products;
        }

        public bool placeOrder(string userId)
        {
            List<CartProductItem> cartItems = _shoppingCartItemRepository.FindByCondition(p => p.UserId == userId).ToList();
            if (cartItems.Count > 0)
            {
                double totalPrice = 0;
                Order order = new Order()
                {
                    UserId = userId,
                    Date = DateTime.Now,
                    OrderStatusId = 2,
                    TotalPrice = 0
                };
                _orderRepository.Create(order);
                _orderRepository.Save();
                bool saveOrder = false;

                cartItems.ForEach(cartItem => {
                    if (cartItem.Quantity > 0)
                    {
                        saveOrder = true;
                        double localPrice = _productRepository.FindByCondition(p => p.Id == cartItem.ProductId).ToList().ElementAt(0).Price;
                        _orderProductItemRepository.Create(
                             new OrderProductItem
                             {
                                 ProductId = cartItem.ProductId,
                                 Quantity = cartItem.Quantity,
                                 Price = localPrice,
                                 OrderId = order.Id
                             });
                        totalPrice += localPrice * cartItem.Quantity;
                    }
                });

                if (!saveOrder)
                {
                    _orderRepository.Delete(order);
                    _orderRepository.Save();
                }
                else
                {
                    order.TotalPrice = totalPrice;
                    _orderRepository.Update(order);
                    _orderRepository.Save();

                    cartItems.ForEach(cartItem => _shoppingCartItemRepository.Delete(cartItem));
                    _orderProductItemRepository.Save();
                    _shoppingCartItemRepository.Save();
                    User user = _userService.GetUser(userId);
                }

                return saveOrder;
            }

            return false;
        }

        public void RemoveProductInCart(string userId, int productId)
        {
            _shoppingCartItemRepository.Delete(_shoppingCartItemRepository.FindByCondition(
                p => p.ProductId == productId && p.UserId == userId).ToList().ElementAt(0));
            _shoppingCartItemRepository.Save();
        }

        public void Remove(string userId)
        {
            List<CartProductItem> cartItems = _shoppingCartItemRepository.FindByCondition(p => p.UserId == userId).ToList();
            cartItems.ForEach(ci => _shoppingCartItemRepository.Delete(ci));
            _shoppingCartItemRepository.Save();
        }

        public ShoppingCartModelView getCartModelView(string id)
        {
            List<CartProductItem> cartItems = _shoppingCartItemRepository.FindByCondition(p => p.UserId == id).ToList();
            if (cartItems != null)
            {
                List<ItemProductModelView> viewProducts = new List<ItemProductModelView>();
                cartItems.ForEach(product =>
                {
                    Product currentProduct = _productRepository.FindByCondition(p => product.ProductId == p.Id).FirstOrDefault();
                    viewProducts.Add(new ItemProductModelView()
                    {
                        Id = product.ProductId,
                        Quantity = product.Quantity,
                        Photo = currentProduct.Photo,
                        Price = currentProduct.Price,
                        ProductName = currentProduct.Name
                    });
                });
                return new ShoppingCartModelView()
                {
                    Id = id,
                    Products = viewProducts
                };
            }

            return null;
        }
    }
}
