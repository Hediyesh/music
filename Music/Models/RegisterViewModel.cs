using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Music
{
    public class RegisterViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "firstname")]
        [Required(ErrorMessage = "please enter your {0}")]
        public string FirstName { get; set; }

        [Display(Name = "lastname")]
        [Required(ErrorMessage = "please enter your {0}")]
        public string LastName { get; set; }

        [Display(Name = "username")]
        [Required(ErrorMessage = "please enter your {0}")]
        public string UserName { get; set; }

        [Display(Name = "password")]
        [Required(ErrorMessage = "please enter your {0}")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "repeat password")]
        [Required(ErrorMessage = "please {0}")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="passwords do not match!")]
        public string RePassword { get; set; }

        [Display(Name = "email")]
        [Required(ErrorMessage = "please enter your {0}")]
        [EmailAddress(ErrorMessage ="email is not correct")]
        public string Email { get; set; }

    }
}