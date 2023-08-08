using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Music
{
    public class UserViewModel
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "firstname")]
        [Required(ErrorMessage = "please enter your {0}")]
        public string firstName { get; set; }
        [Display(Name = "lastname")]
        [Required(ErrorMessage = "please enter your {0}")]
        public string lastName { get; set; }

        [Display(Name ="bio")]
        [Required(ErrorMessage = "please enter your {0}")]
        public string bio { get; set; }

        [Display(Name = "username")]
        [Required(ErrorMessage = "please enter your {0}")]
        public string userName { get; set; }
    }
}