using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AutoPartsWarehouse.ViewModels
{
    public class ChangeRoleViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public List<IdentityRole> AllRoles { get; set; } = new List<IdentityRole>();
        public IList<string> UserRoles { get; set; } = new List<string>();

        public ChangeRoleViewModel() { }
    }
}