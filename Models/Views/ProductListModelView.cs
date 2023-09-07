namespace ProjectLab.Models.View
{
    public class ProductListModelView
    {
        public IEnumerable<ProductModelView> Products { get; set; }

        public string PageTitle { get; set; }

        public int PageProductTypeId { get; set; }
    }
}
