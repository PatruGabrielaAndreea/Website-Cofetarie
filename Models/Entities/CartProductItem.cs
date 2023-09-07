using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectLab.Models.Entities
{
    [Table("CartProductItem")]
    public class CartProductItem
    {
        public string UserId { get; set; }

        public virtual User? User { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        [Range(1,int.MaxValue)]
        public int Quantity { get; set; }

    }
}
