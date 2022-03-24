using Online_Art_Gallery.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Art_Gallery.Areas.Admin.Controllers
{
    public class ArtworkController : BaseController
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();
        // GET: Admin/Artwork
        public ActionResult Index()
        {
            ViewData["artists"] = entities.Artists.ToList();
            ViewData["categorys"] = entities.Categories.ToList();
            ViewData["artworks"] = entities.Artworks.ToList();
            return View();
        }

        // GET: Admin/Artwork/Create
        public ActionResult Create()
        {
            ViewData["artists"] = entities.Artists.ToList();
            ViewData["categorys"] = entities.Categories.ToList();
            return View();
        }
        // POST: Admin/Artwork/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string name, HttpPostedFileBase picture, float? price, float? sale_price, DateTime? year, string size, string type, string technique, int? id_artist, int? id_category, string descreption, bool status)
        {
            //Validation Data
            if (name == "")
            {
                TempData["name-validation"] = "Please Enter Name..!";
                return RedirectToAction("Create");
            }
            if (picture == null)
            {
                TempData["picture-validation"] = "Please Enter Picture..!";
                return RedirectToAction("Create");
            }
            if (price == null)
            {
                TempData["price-validation"] = "Please Enter Price..!";
                return RedirectToAction("Create");
            }
            if (year == null)
            {
                TempData["year-validation"] = "Please Enter Year..!";
                return RedirectToAction("Create");
            }
            if (size == "")
            {
                TempData["size-validation"] = "Please Enter Size..!";
                return RedirectToAction("Create");
            }
            if (type == "")
            {
                TempData["type-validation"] = "Please Enter Type..!";
                return RedirectToAction("Create");
            }
            if (technique == "")
            {
                TempData["technique-validation"] = "Please Enter Technique..!";
                return RedirectToAction("Create");
            }
            if (id_artist == null)
            {
                TempData["id_artist-validation"] = "Please Enter Id_Artist..!";
                return RedirectToAction("Create");
            }
            if (id_category == null)
            {
                TempData["id_category-validation"] = "Please Enter Id_Category..!";
                return RedirectToAction("Create");
            }
            if (descreption == "")
            {
                TempData["descreption-validation"] = "Please Enter Descreption..!";
                return RedirectToAction("Create");
            }

            //Check Image
            var filename = picture.FileName;
            if (filename.ToLower().EndsWith("jpg") || filename.ToLower().EndsWith("png"))
            {
                if (!Directory.Exists(Server.MapPath("~/Content/Images/Artwork")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Content/Images/Artwork"));
                    picture.SaveAs(Server.MapPath("~/Content/Images/Artwork/" + filename));
                }
                else
                {
                    picture.SaveAs(Server.MapPath("~/Content/Images/Artwork/" + filename));
                }
            }
            else
            {
                TempData["picture-validation"] = "Photos must be of the correct type: jpg, png";
                return RedirectToAction("Create");
            }

            //Check Name
            var check_name = entities.Artworks.FirstOrDefault(s => s.Name == name);
            if (check_name != null)
            {
                TempData["name-validation"] = "Artist Name already exists";
                return RedirectToAction("Create");
            }

            //Add
            try
            {
                Artwork artwork = new Artwork();
                var Id_admin = Session["Id_Admin"];
                artwork.Name = name;
                artwork.Picture = filename;
                artwork.Price = price;
                if (sale_price != null)
                {
                    artwork.Sale_Price = sale_price;
                }
                else
                {
                    artwork.Sale_Price = 0;
                }
                artwork.Year = year;
                artwork.Size = size;
                artwork.Type = type;
                artwork.Technique = type;
                artwork.Owner = int.Parse(Id_admin.ToString());
                artwork.Id_Artist = id_artist;
                artwork.Id_Category = id_category;
                artwork.Descreption = descreption;
                artwork.Status = status;
                artwork.Total_Rating = 0;
                artwork.Total_Rating_Points = 0;

                entities.Artworks.Add(artwork);
                entities.SaveChanges();

                TempData["Success"] = "Create Success !";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Error"] = "Create Failed..!";
                return RedirectToAction("Create");
            }
        }

        // GET: Admin/Artwork/Update
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var art = entities.Artworks.Find(id);
            if (art == null)
            {
                return RedirectToAction("Index");
            }
            ViewData["artists"] = entities.Artists.ToList();
            ViewData["categorys"] = entities.Categories.ToList();
            ViewBag.Artwork = art;
            return View();
        }
        // POST: Admin/Artwork/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Artwork validation, int id, string name, HttpPostedFileBase picture, float? price, float? sale_price, DateTime? year, string size, string type, string technique, int? id_artist, int? id_category, string descreption, bool status)
        {
            //Validation Data
            if (name == "")
            {
                TempData["name-validation"] = "Please Enter Name..!";
                return RedirectToAction("Update", new { Id = id });
            }
            if (price == null)
            {
                TempData["price-validation"] = "Please Enter Price..!";
                return RedirectToAction("Update", new { Id = id });
            }
            if (year == null)
            {
                TempData["year-validation"] = "Please Enter Year..!";
                return RedirectToAction("Update", new { Id = id });
            }
            if (size == "")
            {
                TempData["size-validation"] = "Please Enter Size..!";
                return RedirectToAction("Update", new { Id = id });
            }
            if (type == "")
            {
                TempData["type-validation"] = "Please Enter Type..!";
                return RedirectToAction("Update", new { Id = id });
            }
            if (technique == "")
            {
                TempData["technique-validation"] = "Please Enter Technique..!";
                return RedirectToAction("Create");
            }
            if (id_artist == null)
            {
                TempData["id_artist-validation"] = "Please Enter Id_Artist..!";
                return RedirectToAction("Update", new { Id = id });
            }
            if (id_category == null)
            {
                TempData["id_category-validation"] = "Please Enter Id_Category..!";
                return RedirectToAction("Update", new { Id = id });
            }
            if (descreption == "")
            {
                TempData["descreption-validation"] = "Please Enter Descreption..!";
                return RedirectToAction("Update", new { Id = id });
            }

            //Check Image
            var filename = picture == null ? "" : picture.FileName;
            if (filename != "")
            {
                if (filename.ToLower().EndsWith("jpg") || filename.ToLower().EndsWith("png"))
                {
                    if (!Directory.Exists(Server.MapPath("~/Content/Images/Artwork")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Images/Artwork"));
                        picture.SaveAs(Server.MapPath("~/Content/Images/Artwork/" + filename));
                    }
                    else
                    {
                        picture.SaveAs(Server.MapPath("~/Content/Images/Artwork/" + filename));
                    }
                }
                else
                {
                    TempData["picture-validation"] = "Photos must be of the correct type: jpg, png";
                    return RedirectToAction("Update", new { Id = id });
                }
            }

            //Check Name
            var check_name = entities.Artworks.FirstOrDefault(s => s.Id != id && s.Name == name);
            if (check_name != null)
            {
                TempData["name-validation"] = "Artwork Name already exists..!";
                return RedirectToAction("Update", new { Id = id });
            }
            //Update
            try
            {
                Artwork artwork = entities.Artworks.Find(id);
                artwork.Name = name;
                if (filename != "")
                {
                    artwork.Picture = filename;
                }
                artwork.Price = price;
                if (sale_price != null)
                {
                    artwork.Sale_Price = sale_price;
                }
                else
                {
                    artwork.Sale_Price = 0;
                }
                artwork.Year = year;
                artwork.Size = size;
                artwork.Type = type;
                artwork.Technique = technique;
                artwork.Id_Artist = id_artist;
                artwork.Id_Category = id_category;
                artwork.Descreption = descreption;
                artwork.Status = status;

                entities.SaveChanges();

                TempData["Success"] = "Update Success..!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Error"] = "Update Failed..!";
                return RedirectToAction("Index");
            }
        }

        // GET: Artwork/Detail
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var art = entities.Artworks.Find(id);
            if (art == null)
            {
                return RedirectToAction("Index");
            }
            ViewData["artists"] = entities.Artists.ToList();
            ViewData["categorys"] = entities.Categories.ToList();
            ViewBag.User = entities.Users.Find(art.Owner);
            ViewBag.Artwork = art;
            return View();
        }

        // GET: Admin/Artwork/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var artwork = entities.Artworks.Find(id);
            if (artwork == null)
            {
                TempData["Error"] = "Delete Failed";
                return RedirectToAction("Index");
            }

            var order = entities.OrderDetails.FirstOrDefault(s => s.Id_Artwork == artwork.Id);
            var cart = entities.Carts.FirstOrDefault(s => s.Id_Artwork == artwork.Id);
            var wishlist = entities.Wishlists.FirstOrDefault(s => s.Id_Artwork == artwork.Id);
            var rating = entities.Ratings.FirstOrDefault(s => s.Id_Artwork == artwork.Id);
           
            if (order != null)
            {
                TempData["Error"] = "Delete Failed";
                return RedirectToAction("Index");
            }
            if (cart != null)
            {
                TempData["Error"] = "Delete Failed";
                return RedirectToAction("Index");
            }
            if (wishlist != null)
            {
                TempData["Error"] = "Delete Failed";
                return RedirectToAction("Index");
            }
            if (rating != null)
            {
                TempData["Error"] = "Delete Failed";
                return RedirectToAction("Index");
            }

            entities.Artworks.Remove(artwork);
            entities.SaveChanges();
            TempData["Success"] = "Delete Success !";
            return RedirectToAction("Index");
        }

        // GET: Artwork/Show
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var art = entities.Artworks.Find(id);
            if (art == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                art.Status = true;
                entities.SaveChanges();
                TempData["Success"] = "Update Success..!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Error"] = "Update Failed..!";
                return RedirectToAction("Index");
            }
        }
    }
}