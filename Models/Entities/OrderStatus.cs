using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectLab.Models.Entities
{
    [Table("OrderStatus")]
    public class OrderStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }    

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
