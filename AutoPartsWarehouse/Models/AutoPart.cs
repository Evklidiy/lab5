using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPartsWarehouse.Models
{
    public class AutoPart
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Артикул обязателен")]
        [StringLength(50)]
        [Display(Name = "Артикул")]
        public string Article { get; set; } = string.Empty;

        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(100)]
        [Display(Name = "Название")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Производитель")]
        public string? Manufacturer { get; set; }

        [Display(Name = "Совместимость")]
        public string? CompatibleModels { get; set; }

        [Display(Name = "Цена закупки")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PurchasePrice { get; set; }

        [Display(Name = "Цена продажи")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal SalePrice { get; set; }
    }
}