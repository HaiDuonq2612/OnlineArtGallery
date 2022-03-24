using Online_Art_Gallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Online_Art_Gallery.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();
        // GET: Admin/HomeAdmin
        public ActionResult Index()
        {
            if (Session["Id_Admin"] == null)
            {
                return RedirectToAction("Login");
            }

            ViewData["Categories"] = entities.Categories.ToList();
            ViewData["Artists"] = entities.Artists.ToList();
            ViewData["Artworks"] = entities.Artworks.ToList();
            ViewData["Users"] = entities.Users.Where(x => x.Status == false).ToList();
            ViewData["Exhibitions"] = entities.Exhibitions.ToList();
            ViewData["Orders"] = entities.Orders.ToList();
            ViewData["OrderExhibitions"] = entities.OrderExhibitions.Where(x => x.Status == true).ToList();
            ViewData["PaymentMethods"] = entities.PaymentMethods.ToList();
            ViewData["Blogs"] = entities.Blogs.ToList();
            ViewData["Banners"] = entities.Banners.ToList();
            ViewData["Ratings"] = entities.Ratings.ToList();
            return View();

        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string login_name, string password)
        {
            var f_password = GetMD5(password);
            var data = entities.Users.Where(s => s.Login_Name.Equals(login_name) && s.Login_Password.Equals(f_password)).ToList();
            if (data.Count() > 0)
            {
                if (data.FirstOrDefault().Status == true)
                {
                    Session["Id_Admin"] = data.FirstOrDefault().Id;
                    Session["Name_Admin"] = data.FirstOrDefault().Name;

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Login Failed !";
                    return RedirectToAction("Login");

                }

            }
            else
            {
                TempData["Error"] = "Login Failed !";
                return RedirectToAction("Login");
            }
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        
    }
}