using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectLab.Models.Entities
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string UserId { get; set; }

        public virtual User? User { get; set; }

        [ForeignKey("OrderStatus"), Display(Name = "Status")]
        public int OrderStatusId { get; set; }

        [Display(Name = "Status")]
        public virtual OrderStatus? OrderStatus { get; set; }

        [Display(Name = "Total price")]
        public double TotalPrice { get; set; }

        public ICollection<OrderProductItem> OrdersProductItems { get; set; }
    }
}
