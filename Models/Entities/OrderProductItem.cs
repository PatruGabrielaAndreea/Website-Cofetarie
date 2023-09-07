using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectLab.Models.Entities
{
    [Table("OrderProductItem")]
    public class OrderProductItem
    {
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        public virtual Order? Order { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}
