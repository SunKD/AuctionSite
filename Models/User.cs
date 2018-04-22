using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Models
{
    public class User : BaseEntity
    {   
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public double Wallet {get; set;} = 1000;
        // public List<Product> SellingProducts = new List<Product>();
        public List<Auction> JoinedAcutions = new List<Auction>();
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;
    }
}