using Online_Art_Gallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Art_Gallery.Controllers
{
    public class OrderHistoryController : BaseController
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();
        // GET: OrderHistory
        public ActionResult Index()
        {
            int Id_User = int.Parse(Session["Id"].ToString());
            ViewData["orders"] = entities.Orders.Where(x => x.Id_User == Id_User).ToList();
            ViewData["pending_orders"] = entities.Orders.Where(x => x.Status == 0 && x.Id_User == Id_User).ToList();
            ViewData["processed_orders"] = entities.Orders.Where(x => x.Status == 1 && x.Id_User == Id_User).ToList();
            ViewData["canceled_orders"] = entities.Orders.Where(x => x.Status == 2 && x.Id_User == Id_User).ToList();
            return View();
        }
        // GET: OrderHistory/Detail
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Orders = entities.Orders.Find(id);
            var order = entities.Orders.Find(id);
            ViewData["orderdetails"] = entities.OrderDetails.Where(x => x.Id_Order == order.Id).ToList();
            ViewData["artworks"] = entities.Artworks.ToList();
            return View();
        }
        // POST: OrderHistory/CancelOrder
        public ActionResult CancelOrder(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var order = entities.Orders.Find(id);
            order.Status = 2;
            entities.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: OrderHistory/Exhibition
        public ActionResult Exhibition()
        {
            int Id_User = int.Parse(Session["Id"].ToString());

            var exhibitions = entities.Exhibitions.OrderByDescending(x => x.Id).ToList();
            OrderExhibition order = new OrderExhibition();

            List<OrderExhibition> lstorder = new List<OrderExhibition>();

            foreach (var ex in exhibitions)
            {
                if (ex.OrderExhibitions.Where(x => x.Id_User == Id_User).Count() > 0)
                {
                    order = entities.OrderExhibitions.Where(x => x.Id_User == Id_User && x.Id_Exhibition == ex.Id).OrderByDescending(x => x.Bet_Price).First();
                    lstorder.Add(order);
                }
                
            }

            ViewData["orders"] = lstorder;

            return View();
        }
    }
}