using Online_Art_Gallery.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Art_Gallery.Controllers
{
    public class ArtworkController : Controller
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();
        // GET: Artwork
        public ActionResult Index(int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            var artworks = (from art in entities.Artworks where art.Status == true select art).OrderByDescending(x => x.Id);

            List<Category> lstCate = new List<Category>();
            foreach (var a in artworks.Select(x => x.Id_Category).Distinct())
            {
                var categories = entities.Categories.Where(x => x.Id == a).ToList();
                foreach (var item in categories)
                {
                    lstCate.Add(item);
                }
            }

            ViewData["categorys"] = lstCate;
            ViewData["artists"] = entities.Artists.ToList();

            ViewData["wishlists"] = entities.Wishlists.ToList();
            return View(artworks.ToPagedList(pageNumber, pageSize));
        }

        // GET: ArtworkUser/Detail
        public ActionResult Detail(int id)
        {
            ViewData["categorys"] = entities.Categories.ToList();
            ViewData["ratings"] = entities.Ratings.ToList();
            ViewData["users"] = entities.Users.ToList();
            ViewBag.Artwork = entities.Artworks.Find(id);
            return View();
        }

        // POST: ArtworkUser/RatingArtwork
        [HttpPost]
        public ActionResult RatingArtwork(int? id_artwork, int? rating_number, string descreption)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (descreption == null)
            {
                TempData["ErrorDescreption"] = "Descreption Null !";
                return RedirectToAction("Detail", new { id = id_artwork });
            }

            int Id_User = int.Parse(Session["Id"].ToString());
            DateTime date = DateTime.Now;
            Rating rating = new Rating();
            rating.Id_User = Id_User;
            rating.Id_Artwork = id_artwork;
            rating.Rating_Number = rating_number;
            rating.Published_Date = date;
            rating.Descreption = descreption;
            rating.Status = false;
            entities.Ratings.Add(rating);
            entities.SaveChanges();

            var artwork = entities.Artworks.Find(id_artwork);
            artwork.Total_Rating += 1;
            artwork.Total_Rating_Points += rating_number;
            entities.SaveChanges();
            TempData["Success"] = "Rating Success !";
            return RedirectToAction("Detail", new { id = id_artwork });
        }


        // GET: ArtworkUser/SearchByName
        public ActionResult SearchByName(string name, int? page)
        {
            if (name == "")
            {
                TempData["Error"] = "Please Enter Name..!";
                return RedirectToAction("Index");
            }
            var artworks = entities.Artworks.Where(x => x.Name.Contains(name) && x.Status == true).OrderByDescending(x => x.Id).ToList();
            if (artworks.Count() <= 0)
            {
                TempData["Error"] = "No Data Item..!";
                return RedirectToAction("Index");
            }
            List<Category> lstCate = new List<Category>();
            foreach (var a in artworks.Select(x => x.Id_Category).Distinct())
            {
                var categories = entities.Categories.Where(x => x.Id == a).ToList();
                foreach (var item in categories)
                {
                    lstCate.Add(item);
                }
            }
            ViewData["categorys"] = lstCate;
            ViewData["artworks"] = artworks;
            ViewData["artists"] = entities.Artists.ToList();

            return View();

        }
        // GET: ArtworkUser/SortBy
        public ActionResult SortBy(int id)
        {
            List<Category> lstCate = new List<Category>();
            if (id == 1)
            {
                var artworks = entities.Artworks.Where(x => x.Status == true).OrderBy(x => x.Price).ThenBy(x => x.Sale_Price).ToList();
                foreach (var a in artworks.Select(x => x.Id_Category).Distinct())
                {
                    var categories = entities.Categories.Where(x => x.Id == a).ToList();
                    foreach (var item in categories)
                    {
                        lstCate.Add(item);
                    }
                }
                ViewData["categorys"] = lstCate;
                ViewData["artworks"] = artworks;
                ViewData["artists"] = entities.Artists.ToList();
                return View();
            }
            else
            {
                var artworks = entities.Artworks.Where(x => x.Status == true).OrderByDescending(x => x.Price).ThenByDescending(x => x.Sale_Price).ToList();
                foreach (var a in artworks.Select(x => x.Id_Category).Distinct())
                {
                    var categories = entities.Categories.Where(x => x.Id == a).ToList();
                    foreach (var item in categories)
                    {
                        lstCate.Add(item);
                    }
                }
                ViewData["categorys"] = lstCate;
                ViewData["artworks"] = artworks;
                ViewData["artists"] = entities.Artists.ToList();
                return View();
            }
            
        }
        // GET: ArtworkUser/SearchByPrice
        public ActionResult SearchByPrice(string price)
        {
            var s_price = price.Split('-' );
            var start_price = float.Parse(s_price[0].Replace("$", ""));
            var end_price = float.Parse(s_price[1].Replace("$", ""));

            var artworks = entities.Artworks.Where(x => x.Sale_Price > 0 ? x.Sale_Price >= start_price && x.Sale_Price <= end_price && x.Status == true : x.Price >= start_price && x.Price <= end_price && x.Status == true).ToList();
            if (artworks.Count() <= 0)
            {
                TempData["Error"] = "No Data Item..!";
                return RedirectToAction("Index");
            }
            List<Category> lstCate = new List<Category>();
            foreach (var a in artworks.Select(x => x.Id_Category).Distinct())
            {
                var categories = entities.Categories.Where(x => x.Id == a).ToList();
                foreach (var item in categories)
                {
                    lstCate.Add(item);
                }
            }
            ViewData["categorys"] = lstCate;
            ViewData["artworks"] = artworks;
            ViewData["artists"] = entities.Artists.ToList();
            return View();
        }
        // GET: ArtworkUser/SearchByArtist
        public ActionResult SearchByArtist(int id)
        {
            var artist = entities.Artists.Find(id);
            var artworks = entities.Artworks.Where(x => x.Id_Artist == artist.Id && x.Status == true).OrderByDescending(x => x.Id).ToList();
            if (artworks.Count() <= 0)
            {
                TempData["Error"] = "No Data Item..!";
                return RedirectToAction("Index");
            }
            List<Category> lstCate = new List<Category>();
            foreach (var a in artworks.Select(x => x.Id_Category).Distinct())
            {
                var categories = entities.Categories.Where(x => x.Id == a).ToList();
                foreach (var item in categories)
                {
                    lstCate.Add(item);
                }
            }
            ViewData["categorys"] = lstCate;
            ViewData["artworks"] = artworks;
            ViewData["artists"] = entities.Artists.ToList();
            return View();
        }

        // GET: ArtworkUser/SearchByCategory
        public ActionResult SearchByCategory(int? id)
        {
            var category = entities.Categories.Find(id);
            var artworks = entities.Artworks.Where(x => x.Id_Category == category.Id && x.Status == true).OrderByDescending(x => x.Id).ToList();
            if (artworks.Count() <= 0)
            {
                TempData["Error"] = "No Data Item..!";
                return RedirectToAction("Index");
            }
            List<Category> lstCate = new List<Category>();
            foreach (var a in artworks.Select(x => x.Id_Category).Distinct())
            {
                var categories = entities.Categories.Where(x => x.Id == a).ToList();
                foreach (var item in categories)
                {
                    lstCate.Add(item);
                }
            }
            ViewData["categorys"] = lstCate;
            ViewData["artworks"] = artworks;
            ViewData["artists"] = entities.Artists.ToList();
            return View();
        }


    }
}