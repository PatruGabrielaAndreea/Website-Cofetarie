using ProjectLab.Models.View;
using ProjectLab.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ProjectLab.Controllers
{
    [Authorize]
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        private readonly IUserService _userService;

        public ShoppingCartsController(IShoppingCartService shoppingCartService,
            IUserService userService)
        {
            _shoppingCartService = shoppingCartService;
            _userService = userService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (_userService.IsUserLoggedIn(User))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ShoppingCartModelView cartModel = _shoppingCartService.getCartModelView(userId);
                if (cartModel.Products != null && cartModel.Products.Count() > 0)
                {
                    return View(cartModel);
                }
                else
                {
                    return View("~/Views/ShoppingCarts/EmptyCart.cshtml");
                }
            }
            else
            {
                return RedirectToAction("Login", "Accounts");
            }

        }

        // GET: ShoppingCartModelViews
        public async Task<IActionResult> ThankYou()
        {
            return View("~/Views/ShoppingCarts/ThankYou.cshtml");
        }

        public IActionResult IncreaseProductQuantity(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _shoppingCartService.increaseProductQuantity(userId, productId);
            return RedirectToAction("Index");
        }

        public IActionResult DecreaseProductQuantity(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _shoppingCartService.decreaseProductQuantity(userId, productId);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteProduct(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _shoppingCartService.RemoveProductInCart(userId, productId);
            return RedirectToAction("Index");
        }

        public IActionResult PlaceOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool wasPlaced = _shoppingCartService.placeOrder(userId);
            return RedirectToAction(wasPlaced ? "ThankYou" : "Index");
        }
    }
}
