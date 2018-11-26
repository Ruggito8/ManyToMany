using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModel
{
    public class Registration
    {
        [Required(ErrorMessage = "User must have a name")]
        [MaxLength(25)]
        public string Name { get; set; }
    
        [Required(ErrorMessage = "User must have a valid email")]
        [MaxLength(25)]
        public string Email { get; set; }

        [Required(ErrorMessage = "User must have a valid password (at least 3 characters)")]
        [MinLength(3)]
        public string Password { get; set; }
    }
}
