using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class AccountModel
    {
        public string Id { get; set; }
        [Display(Name = "Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Name")]
        public string? LastName { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number!")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number!")]
        public string? PhoneNumber { get; set; }


        [Display(Name = "Date of Birth")]
        public DateTime? BirthDate { get; set; }

        public string? Biography { get; set; }

        public string? ProfileImg { get; set; }

        public string? Sex { get; set; }
        public string? ParentEmail { get; set; }
        public string? DeviceToken { get; set; } = "";
    }

    public class AccountModelGetList
    {
        public string Id { get; set; }
        [Display(Name = "Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Name")]
        public string? LastName { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number!")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number!")]
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? ParentEmail { get; set; }
        public string? Status { get; set; }
        public string? Role { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime? BirthDate { get; set; }

        public string? Biography { get; set; }

        public string? ProfileImg { get; set; }

        public string? Sex { get; set; }

    }
}
