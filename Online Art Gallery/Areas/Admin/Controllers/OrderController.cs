using Online_Art_Gallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Art_Gallery.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();

        // GET: Order/OrderArtwork
        public ActionResult OrderArtwork()
        {
            var order = entities.Orders.OrderByDescending(x => x.Order_Date).ToList();
            ViewData["orders"] = order;
            ViewData["paymentmethods"] = entities.PaymentMethods.ToList();
            ViewData["users"] = entities.Users.ToList();
            return View();
        }

        // GET: Order/OrderArtworkDetail
        public ActionResult OrderArtworkDetail(int? id)
        {
            ViewBag.Order = entities.Orders.Find(id);
            ViewData["paymentmethods"] = entities.PaymentMethods.ToList();
            ViewData["users"] = entities.Users.ToList();
            ViewData["artworks"] = entities.Artworks.ToList();
            ViewData["orderdetails"] = entities.OrderDetails.ToList();
            return View();
        }

        // GET: Order/OrderArtwork/Processing
        public ActionResult OrderArtworkProcessing(int? id)
        {
            var order = entities.Orders.Find(id);
            order.Status = 1;

            var OrderDetails = entities.OrderDetails.Where(x => x.Id_Order == order.Id).ToList();
            foreach (var detail in OrderDetails)
            {
                var Artworks = entities.Artworks.Where(x => x.Id == detail.Id_Artwork).ToList();
                foreach (var artwork in Artworks)
                {
                    artwork.Owner = order.Id_User;
                    artwork.Status = false;
                }

            }

            entities.SaveChanges();
            TempData["Success"] = "Order Processed Successfully..!";
            return RedirectToAction("OrderArtwork");
        }

        // GET: Order/OrderExhibitions
        public ActionResult OrderExhibition()
        {
            var exhibitions = entities.Exhibitions.OrderByDescending(x => x.Start_Date).ToList();
            List<Exhibition> lst = new List<Exhibition>();


            foreach (var ex in exhibitions)
            {
                var order = entities.OrderExhibitions.Where(x => x.Id_Exhibition == ex.Id);
                if (order.Count() > 0)
                {
                    lst.Add(ex);
                }
            }
            ViewData["exhibitions"] = lst;
            return View();
        }
        // GET: Order/OrderExhibitonDetail
        public ActionResult OrderExhibitionDetail(int id)
        {
            ViewBag.Exhibition = entities.Exhibitions.Find(id);
            ViewData["orders_true"] = entities.OrderExhibitions.Where(x => x.Id_Exhibition == id && x.Status == true).ToList();

            ViewData["orders_false"] = entities.OrderExhibitions.Where(x => x.Id_Exhibition == id).OrderByDescending(o => o.Bet_Price).Take(3).ToList();
            return View();
        }
        // GET: Order/OrderExhibitonDetail/Processing
        public ActionResult OrderExhibitionProcessing(int id)
        {
            var exhibition = entities.Exhibitions.Find(id);
            DateTime date = DateTime.Now;
            DateTime date_end = Convert.ToDateTime(exhibition.End_Date);

            TimeSpan Time = date.Subtract(date_end);
            
            double end = Time.TotalDays;
            if (end < 0)
            {
                TempData["Error"] = "The Event is not over yet..!";
                return RedirectToAction("OrderExhibitionDetail", new { id = id });
            }
            var order = entities.OrderExhibitions.Where(x => x.Id_Exhibition == id).OrderByDescending(o => o.Bet_Price).First();
            if (order == null )
            {
                TempData["Error"] = "No Order Exhibitions..!";
                return RedirectToAction("OrderExhibitionDetail", new { id = id });
            }
            exhibition.Status = true;
            exhibition.Owner = order.Id_User;
            order.Status = true;

            entities.SaveChanges();
            TempData["Success"] = "Order Processed Successfully..!";
            return RedirectToAction("OrderExhibitionDetail", new { id = id });
        }
    }
}