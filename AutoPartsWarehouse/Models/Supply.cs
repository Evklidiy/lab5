using System;
using System.ComponentModel.DataAnnotations;

namespace AutoPartsWarehouse.Models
{
    public class Supply
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Дата поставки")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        [Display(Name = "Статус")]
        public string Status { get; set; } = "Ожидается"; // Ожидается, Получена

        // Внешний ключ
        [Display(Name = "Поставщик")]
        public int SupplierId { get; set; }

        // Навигационное свойство
        public Supplier? Supplier { get; set; }
    }
}