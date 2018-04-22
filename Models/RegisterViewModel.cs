using System.ComponentModel.DataAnnotations;
using System;

namespace Dashboard.Models
{
    public class RegisterViewModel : BaseEntity
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Firstname should be more than 2 characters")]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Lastname should be more than 4 characters")]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string LastName { get; set; }

        [Display(Name = "User Name")]
        [Required]
        [MinLength(3)]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password should be more than 8 characters")]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        [DataType(DataType.Password)]
        public string ConfirmedPW { get; set; }

    }
}