using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectLab.Models.Entities
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string? FilePath { get; set; }

        [Required, Range(0, double.MaxValue)]
        public double Price { get; set; }

        public bool isDeleted { get; set; }

        public byte[]? Photo { get; set; }

        public ICollection<CartProductItem> CartProductItems { get; set; } = new List<CartProductItem>();

        public ICollection<OrderProductItem> OrdersProductItems { get; set; } = new List<OrderProductItem>();

        public ProductType? Category { get; set; }
    }
}
