using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectLab.Models.Entities
{
    [Table("ProductType")]
    public class ProductType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } 

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
