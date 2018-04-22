using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Models
{
    public class Product : BaseEntity
    {   
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double StartingBid { get; set; }
        public DateTime EndDate {get; set;}
        public int SellerID {get; set;}
        public User Seller {get; set;}
        public List<Auction> Auctions = new List<Auction>();
        public string RemainingDate{
            get{
                // return (EndDate - DateTime.Now).ToString("d") + "Days"; 
                double time = this.EndDate.Subtract(DateTime.Now).TotalDays;
                return (int)time + " day(s)";
            }
        }
        
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;
    }
}