using ProjectLab.Models.Entities;
using ProjectLab.Models.View;

namespace ProjectLab.Services.Interface
{
    public interface IShoppingCartService
    {
        void Remove(string userId);

        List<Product> getUserShoppingCartProducts(string userId);

        void AddProductInCart(string userId, int productId);

        void RemoveProductInCart(string userId, int productId);

        void EditProductCartQuantity(string userId, int productId, int newQuantity);

        bool placeOrder(string userId);

        void increaseProductQuantity(string userId, int productId);

        void decreaseProductQuantity(string userId, int productId);

        ShoppingCartModelView getCartModelView(string id);
    }
}
