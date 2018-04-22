using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Models
{
    public class Auction : BaseEntity
    {
        public int AuctionID { get; set; }
        public int ItemID { get; set; }
        public Product Item { get; set; }
        public int TopbidderID { get; set; }
        public User Topbidder { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;
    }
}