using Online_Art_Gallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Art_Gallery.Controllers
{
    public class CartController : BaseController
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();
        

        // GET: Cart
        public ActionResult Index()
        {
            int Id_User = int.Parse(Session["Id"].ToString());

            ViewData["carts"] = entities.Carts.Where(x => x.Id_User == Id_User).ToList();

            ViewData["artworks"] = entities.Artworks.ToList();
            return View();
        }
        // GET: Cart/Add
        public ActionResult Add(int id)
        {
            var check_art = entities.Artworks.Find(id);
            if (check_art == null)
            {
                TempData["Error"] = "Add To Cart Failed..!";
                return RedirectToAction("Index");
            }
            if (check_art.Status == false)
            {
                TempData["Error"] = "Add To Cart Failed..!";
                return RedirectToAction("Index");
            }
            var Id_User = Session["Id"];
            int id_user = int.Parse(Id_User.ToString());
            
            var checkcart = entities.Carts.FirstOrDefault(s => s.Id_Artwork == id && s.Id_User == id_user);
            if (checkcart != null)
            {
                TempData["Error"] = "Artworks already in the cart..!";
                return RedirectToAction("Index");
            }

            Cart cart = new Cart();
            cart.Id_User = int.Parse(Id_User.ToString());
            cart.Id_Artwork = id;
            cart.Status = true;

            entities.Carts.Add(cart);
            entities.SaveChanges();
            TempData["Success"] = "Artworks added to cart successfully..!";
            return RedirectToAction("Index");
        }
        // GET: Cart/Delete
        public ActionResult Delete(int id)
        {
            var cart = entities.Carts.Find(id);
            if (cart == null)
            {
                TempData["Error"] = "Delete Failed..!";
                return RedirectToAction("Index");
            }
            entities.Carts.Remove(cart);
            entities.SaveChanges();

            TempData["Success"] = "Delete Success..!";
            return RedirectToAction("Index");
        }
    }
}