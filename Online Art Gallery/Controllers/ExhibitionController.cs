using Online_Art_Gallery.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Art_Gallery.Controllers
{
    public class ExhibitionController : Controller
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();
        // GET: ExhibitionUser
        public ActionResult Index(int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            var exhibitions = (from ex in entities.Exhibitions select ex).OrderByDescending(x => x.Id);
            return View(exhibitions.ToPagedList(pageNumber, pageSize));
        }
        // GET: ExhibitionUser/Detail
        public ActionResult Detail(int? id)
        {
            ViewBag.Exhibition = entities.Exhibitions.Find(id);
            ViewData["users"] = entities.Users.ToList();
            ViewData["orders"] = entities.OrderExhibitions.Where(x => x.Id_Exhibition == id).OrderByDescending(o => o.Bet_Price).Take(3).ToList();
            return View();
        }
        // POST: ExhibitionUser/Order
        [HttpPost]
        public ActionResult OrderExhibition(int? id, float? bet_price)
        {
            var Id_User = Session["Id"];
            if (Id_User == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var exhibition = entities.Exhibitions.Find(id);

            DateTime date = DateTime.Now;
            DateTime date_start = Convert.ToDateTime(exhibition.Start_Date);
            DateTime date_end = Convert.ToDateTime(exhibition.End_Date);
            TimeSpan TimeNow = date.Subtract(date_start);
            TimeSpan Time = date.Subtract(date_end);
            double start = TimeNow.TotalDays;
            double end = Time.TotalDays;

            if (start < 0)
            {
                TempData["Error"] = "The event hasn't happened yet !";
                return RedirectToAction("Detail", new { id = id });
            }
            if (end > 0)
            {
                TempData["Error"] = "Event has ended !";
                return RedirectToAction("Detail", new { id = id });
            }
            if (bet_price == null)
            {
                TempData["Error"] = "Please Enter Bet Price!";
                return RedirectToAction("Detail", new { id = id });
            }
            var order_price = entities.OrderExhibitions.Where(p => p.Bet_Price > 0 && p.Id_Exhibition == exhibition.Id).Max(p => p.Bet_Price);
            if (bet_price <= order_price)
            {
                TempData["Error"] = "Bet Price must be greater than the TOP 1 BET !";
                return RedirectToAction("Detail", new { id = id });
            }
            if (bet_price < exhibition.Starting_Price)
            {
                TempData["Error"] = "Bet Price must be greater than the Starting Price !";
                return RedirectToAction("Detail", new { id = id });
            }

            OrderExhibition order = new OrderExhibition();

            order.Id_User = int.Parse(Id_User.ToString());
            order.Id_Exhibition = id;
            order.Bet_Price = bet_price;
            order.Bet_Date = date;
            order.Status = false;
            entities.OrderExhibitions.Add(order);
            entities.SaveChanges();

            TempData["Success"] = "Place Bet Success !";
            return RedirectToAction("Detail", new { Id = id });
        }
        // GET: ExhibitionUser/SearchByName
        public ActionResult SearchByName(int? page, string name)
        {
            if (name == "")
            {
                TempData["Error"] = "No Data Item..!";
                return RedirectToAction("Index");
            }
            var exhibitions = entities.Exhibitions.Where(x => x.Name.Contains(name)).OrderByDescending(x => x.Id).ToList(); 
            if (exhibitions.Count() <= 0)
            {
                TempData["Error"] = "No Data Item..!";
                return RedirectToAction("Index");
            }
            ViewData["exhibitions"] = exhibitions;
            return View();
        }
        // GET: Exhibition/SearchByDate
        public ActionResult SearchByDate(DateTime? start_date, DateTime? end_date)
        {
            if (start_date == null)
            {
                TempData["start_date-validation"] = "Please Enter Start Date..!";
                return RedirectToAction("Index");
            }
            if (end_date == null)
            {
                TempData["end_date-validation"] = "Please Enter Start Date..!";
                return RedirectToAction("Index");
            }

            DateTime date_start = Convert.ToDateTime(start_date);
            DateTime date_end = Convert.ToDateTime(end_date);
            TimeSpan Time = date_end.Subtract(date_start);
            int days = Time.Days;
            
            var exhibitions = entities.Exhibitions.Where(ex => ex.Start_Date >= start_date && ex.End_Date <= start_date || ex.End_Date >= start_date && ex.Start_Date <= end_date || ex.End_Date >= start_date && ex.End_Date <= end_date).OrderByDescending(x => x.Id).ToList();
            if (exhibitions.Count() <= 0)
            {
                TempData["Error"] = "No Data Item..!";
                return RedirectToAction("Index");

            }
            ViewData["exhibitions"] = exhibitions;
            return View();
        }
    }
}