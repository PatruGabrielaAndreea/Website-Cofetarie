using ProjectLab.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProjectLab.Models.View
{
    public class ProductModelView
    {
        public int ProductId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string? FilePath { get; set; }

        public double Price { get; set; }

        public byte[]? Photo { get; set; }

        public List<string> Labels{ get; set; }

        [Display(Name = "Product Image")]
        public IFormFile? ProductImage { get; set; }
    }
}
