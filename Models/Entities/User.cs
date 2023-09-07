using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ProjectLab.Models.Entities
{
    [Table("User")]
    public class User: IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Address { get; set; }
       
        public DateTime? DateOfBirth { get; set; }

        public byte[]? Photo { get; set; }

        public ICollection<CartProductItem> CartProducts { get; set; } = new List<CartProductItem>();

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
