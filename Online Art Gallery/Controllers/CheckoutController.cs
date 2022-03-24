using Online_Art_Gallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Art_Gallery.Controllers
{
    public class CheckoutController : BaseController
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();
        // GET: Checkout
        public ActionResult Index()
        {
            int Id_User = int.Parse(Session["Id"].ToString());
            ViewBag.User = entities.Users.Find(Id_User);
            ViewData["carts"] = entities.Carts.Where(x => x.Id_User == Id_User && x.Artwork.Status == true).ToList();

            ViewData["artworks"] = entities.Artworks.ToList();
            ViewData["paymentmethods"] = entities.PaymentMethods.ToList();
            return View();
        }

        [HttpPost]
        // GET: Checkout
        public ActionResult Index(string different_address, string descreption, int? id_payment, float? order_price)
        {
            int Id_User = int.Parse(Session["Id"].ToString());
            var carts = entities.Carts.Where(x => x.Id_User == Id_User).ToList();
            if (carts.Count() <= 0)
            {
                TempData["Error"] = "Check Out Failed..!";
                return RedirectToAction("Index");
            }

            if (id_payment == null)
            {
                TempData["id_payment-validation"] = "Please Choose Payment Method..!";
                return RedirectToAction("Index");
            }

            DateTime date = DateTime.Now;

            try
            {
                var order = entities.Orders.Add(new Order
                {
                    Id_User = Id_User,
                    Order_Price = order_price,
                    Order_Date = date,
                    Id_PaymentMethod = id_payment,
                    Different_Address = different_address,
                    Descreption = descreption,
                    Status = 0
                });

                foreach (var item in carts)
                {
                    OrderDetail detail = new OrderDetail();
                    detail.Id_Order = order.Id;
                    detail.Id_Artwork = item.Id_Artwork;
                    entities.OrderDetails.Add(detail);

                    entities.Carts.Remove(item);
                }

                entities.SaveChanges();
                TempData["Success"] = "Check Out Success..!";
                return RedirectToAction("Index", "Cart");
            }
            catch (Exception)
            {
                TempData["Error"] = "Check Out Failed..!";
                return RedirectToAction("Index", "Cart");
            }
        }
    }
}