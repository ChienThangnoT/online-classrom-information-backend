using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class SignUpModel
    {
        public required string AccountPhone { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Email is required!"), EmailAddress(ErrorMessage = "Please enter valid email!")]
        public required string AccountEmail { get; set; }
        public required DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Password must be 7-12 Character")]
        [PasswordPropertyText]
        public string AccountPassword { get; set; } = "";
        [Required(ErrorMessage = "Confirm Password is required!")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("AccountPassword", ErrorMessage = "Password and confirmation does not match!")]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Password must be 7-12 Character")]
        [PasswordPropertyText]
        public string ConfirmAccountPassword { get; set; } = "";
        public string? ParentEmail { get; set; } = "";
    }
}
