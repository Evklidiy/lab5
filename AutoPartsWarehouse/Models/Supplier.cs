using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoPartsWarehouse.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        [Display(Name = "Название поставщика")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Контакты")]
        public string? ContactInfo { get; set; }

        [Display(Name = "Рейтинг")]
        [Range(0, 10)]
        public int Rating { get; set; }

        // Навигационное свойство для связи "один-ко-многим"
        public ICollection<Supply> Supplies { get; set; } = new List<Supply>();
    }
}