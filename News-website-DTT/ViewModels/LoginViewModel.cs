using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace News_website_DTT.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name="Username")]
        public string UserName { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name="Remember me")]
        public bool RememberMe { get; set; }
    }
}
