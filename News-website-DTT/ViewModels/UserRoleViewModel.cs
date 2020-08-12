using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace News_website_DTT.ViewModels
{
    public class UserRoleViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string RoleId { get; set; }
    }
}
