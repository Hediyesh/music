using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Music
{
    public class RecoveryPasswordViewModel
    {
        [Key]
        public int id { get; set; }

        [Display(Name = "new password")]
        [Required(ErrorMessage = "plese enter your {0}")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "repeat password")]
        [Required(ErrorMessage = "plese {0}")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "passwords do not match!")]
        public string RePassword { get; set; }
    }
}