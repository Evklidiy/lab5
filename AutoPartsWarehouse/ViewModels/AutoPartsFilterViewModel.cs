using AutoPartsWarehouse.Models;
using System.Collections.Generic;

namespace AutoPartsWarehouse.ViewModels
{
    public class AutoPartsFilterViewModel
    {
        public IEnumerable<AutoPart> AutoParts { get; set; } = new List<AutoPart>();
        public PageViewModel PageViewModel { get; set; }

        // Поля фильтрации
        public string SearchName { get; set; }       // По названию
        public string SearchManufacturer { get; set; } // По производителю
    }
}