using System.ComponentModel;

namespace ProjectLab.Models.Views
{
    public class ItemProductModelView
    {
        public int Id { get; set; }

        [DisplayName("Name")]
        public string ProductName { get; set; }

        public double Price { get; set; }

        public byte[]? Photo { get; set; }

        [DisplayName("Buc.")]
        public int Quantity { get; set; }
    }
}
