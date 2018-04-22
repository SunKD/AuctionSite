using System.ComponentModel.DataAnnotations;
using System;

namespace Dashboard.Models
{
    public class ProductViewModel : BaseEntity
    {
        [Display(Name = "Product Name")]
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required]
        [MinLength(10)]
        public string Description { get; set; }

        [Display(Name = "Starting Bid")]
        [Required]
        [Range(0.0, Double.MaxValue)]
        public double StartingBid { get; set; }

        [Display(Name = "End Date")]
        [Required]
        [CurrentDate(ErrorMessage = "End Date must be some time in the future")]
        public DateTime EndDate { get; set; }
    }
}