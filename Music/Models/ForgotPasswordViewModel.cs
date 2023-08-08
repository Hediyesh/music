using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Music
{
    public class ForgotPasswordViewModel
    {
        [Display(Name = "email")]
        [Required(ErrorMessage = "plese enter your {0}")]
        [EmailAddress(ErrorMessage = "email is not correct")]
        public string Email { get; set; }
    }
}