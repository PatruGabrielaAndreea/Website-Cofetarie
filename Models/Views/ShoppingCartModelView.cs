using ProjectLab.Models.Views;

namespace ProjectLab.Models.View
{
    public class ShoppingCartModelView
    {
        public string Id { get; set; }

        public List<ItemProductModelView> Products = new List<ItemProductModelView>();

    }
}
