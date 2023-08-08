using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Music
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "please enter your {0}")]
        [EmailAddress(ErrorMessage = "Email is not correct")]
        public string Email { get; set; }

        [Display(Name = "password")]
        [Required(ErrorMessage = "please enter your {0}")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "remember me")]
        public bool RememberMe { get; set; }
    }
}