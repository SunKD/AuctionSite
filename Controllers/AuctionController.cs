using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers
{
    public class AuctionController : Controller
    {
        private DashboardContext _context;

        public AuctionController(DashboardContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            var AllProduct = _context.Products.Include(e => e.Seller).ToList();
            AllProduct.Reverse();
            ViewBag.AllProduct = AllProduct;
            ViewBag.CurrentUserID = (int)HttpContext.Session.GetInt32("CurrentUserID");
            User currentUser = _context.Users.SingleOrDefault(e => e.UserID == (int)HttpContext.Session.GetInt32("CurrentUserID"));
            ViewBag.Wallet = currentUser.Wallet;
            foreach (var item in AllProduct)
            {
                if (item.EndDate <= DateTime.Now)
                {
                    Auction auction = _context.Auctions.Where(d => d.ItemID == item.ProductID).Include(e => e.Topbidder).Include(r => r.Item).ThenInclude(p => p.Seller).SingleOrDefault();

                    if (auction != null)
                    {
                        _context.Auctions.Remove(auction);
                        auction.Topbidder.Wallet -= auction.Item.StartingBid;
                        auction.Item.Seller.Wallet += auction.Item.StartingBid;
                        _context.Products.Remove(auction.Item);
                        _context.SaveChanges();
                        return RedirectToAction("Dashboard");
                    }
                }
            }
            return View("Dashboard");
        }

        [HttpGet]
        [Route("AddNew")]
        public IActionResult AddNew()
        {
            ViewBag.CurrentUserID = (int)HttpContext.Session.GetInt32("CurrentUserID");
            return View("NewAuction");
        }

        [HttpPost]
        [Route("AddProduct")]
        public IActionResult AddProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                int? CurrentUserID = HttpContext.Session.GetInt32("CurrentUserID");

                Product newProduct = new Product
                {
                    Name = model.Name,
                    StartingBid = model.StartingBid,
                    Description = model.Description,
                    EndDate = model.EndDate,
                    SellerID = (int)CurrentUserID
                };

                _context.Products.Add(newProduct);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View("NewAuction");
        }

        [HttpGet]
        [Route("ShowProduct/{itemID}")]
        public IActionResult ShowProduct(int itemID)
        {
            var item = _context.Products.Where(e => e.ProductID == itemID).Include(r => r.Seller).SingleOrDefault();

            var auction = _context.Auctions.Where(e => e.ItemID == itemID).Include(p => p.Topbidder).SingleOrDefault();
            if (auction != null)
            {
                ViewBag.Topbidder = auction;
            }
            else
            {
                ViewBag.Topbidder = null;
            }
            ViewBag.Item = item;
            if (TempData["error"] != null)
            {
                ViewBag.Error = TempData["error"];
            }
            ViewBag.CurrentUserID = (int)HttpContext.Session.GetInt32("CurrentUserID");
            return View("Product");
        }

        [HttpPost]
        [Route("Bid")]
        public IActionResult Bid(int itemID, double amount)
        {
            Product currentItem = _context.Products.Where(e => e.ProductID == itemID).SingleOrDefault();
            User currentUser = _context.Users.Where(e => e.UserID == (int)HttpContext.Session.GetInt32("CurrentUserID")).SingleOrDefault();
            User seller = _context.Users.Where(e => e.UserID == currentItem.SellerID).SingleOrDefault();
            Auction auction = _context.Auctions.Where(e => e.ItemID == itemID).SingleOrDefault();
            if (auction == null)
            {
                Auction newAuction = new Auction
                {
                    ItemID = itemID,
                    TopbidderID = (int)HttpContext.Session.GetInt32("CurrentUserID")
                };
                currentItem = _context.Products.Where(e => e.ProductID == itemID).SingleOrDefault();
                currentItem.StartingBid = amount;
                _context.Auctions.Add(newAuction);
            }
            else if (auction != null)
            {
                if (amount > currentItem.StartingBid && currentUser.Wallet > amount)
                {
                    auction.TopbidderID = (int)HttpContext.Session.GetInt32("CurrentUserID");
                    currentItem.StartingBid = amount;
                }
                else
                {
                    TempData["error"] = "You need to put higher number than current top bid or You need more money to bid!";
                    return RedirectToAction("ShowProduct", new { itemID = itemID });
                }
            }
            else
            {
                ViewBag.Error = "Something Went wrong...";
                return View("Product");
            }
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
