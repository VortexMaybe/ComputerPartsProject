using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerParts.Models
{
    public class HardwareComponent
    {
        public int Id { get; set; }

        [Display(Name = "Име")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Производител")]
        public string Manufacturer { get; set; } = string.Empty;

        [Display(Name = "Категория")]
        public string Type { get; set; } = string.Empty;

        [Display(Name = "Цена")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "Линк към снимка")]
        public string? ImageUrl { get; set; }
    }
}