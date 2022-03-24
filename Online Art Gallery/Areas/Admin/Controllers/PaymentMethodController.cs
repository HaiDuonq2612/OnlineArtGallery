using Online_Art_Gallery.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Art_Gallery.Areas.Admin.Controllers
{
    public class PaymentMethodController : BaseController
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();
        // GET: Admin/PaymentMethod
        public ActionResult Index()
        {
            ViewData["paymentmethods"] = entities.PaymentMethods.ToList();
            return View();
        }
        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Admin/Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string name, string descreption, bool status)
        {
            //Validation Data
            if (name == "")
            {
                TempData["name-validation"] = "Please Enter Name..!";
                return RedirectToAction("Create");
            }
            if (descreption == "")
            {
                TempData["descreption-validation"] = "Please Enter Descreption..!";
                return RedirectToAction("Create");
            }
            //Check Name
            var check_name = entities.PaymentMethods.FirstOrDefault(s => s.Name == name);
            if (check_name != null)
            {
                TempData["name-validation"] = "PaymentMethod Name already exists";
                return RedirectToAction("Create");
            }

            //Add
            try
            {
                PaymentMethod payment = new PaymentMethod();
                payment.Name = name;
                payment.Descreption = descreption;
                payment.Status = status;

                entities.PaymentMethods.Add(payment);
                entities.SaveChanges();

                TempData["Success"] = "Create Success !";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: Admin/Category/Update
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");

            }
            var payment = entities.PaymentMethods.Find(id);
            if (payment == null)
            {
                return RedirectToAction("Index");

            }
            ViewBag.PaymentMethod = payment;
            return View();
        }
        // POST: Admin/Category/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, string name, string descreption, bool status)
        {
            //Validation Data
            if (name == "")
            {
                TempData["name-validation"] = "Please Enter Name..!";
                return RedirectToAction("Update", new { Id = id });
            }
            if (descreption == "")
            {
                TempData["descreption-validation"] = "Please Enter Descreption..!";
                return RedirectToAction("Update", new { Id = id });
            }

            //Check Name
            var check_name = entities.PaymentMethods.FirstOrDefault(s => s.Id != id && s.Name == name);
            if (check_name != null)
            {
                TempData["name-validation"] = "PaymentMethod Name already exists..!";
                return RedirectToAction("Update", new { Id = id });
            }

            try
            {
                var payment = entities.PaymentMethods.Find(id);

                payment.Name = name;
                payment.Descreption = descreption;
                payment.Status = status;

                entities.SaveChanges();

                TempData["Success"] = "Update Success !";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "Update Failed !";
                return RedirectToAction("Index");
            }

        }

        // GET: Admin/Category/Detail
        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");

            }
            var payment = entities.PaymentMethods.Find(id);
            if (payment == null)
            {
                return RedirectToAction("Index");

            }
            ViewBag.PaymentMethod = payment;
            return View();
        }

        // GET: Admin/Category/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");

            }
            var payment = entities.PaymentMethods.Find(id);
            if (payment == null)
            {
                TempData["Error"] = "Delete Failed !";
                return RedirectToAction("Index");

            }
            var order = entities.Orders.FirstOrDefault(s => s.Id_PaymentMethod == payment.Id);
            if (order != null)
            {

                TempData["Error"] = "Delete Failed !";
                return RedirectToAction("Index");

            }

            entities.PaymentMethods.Remove(payment);
            entities.SaveChanges();

            TempData["Success"] = "Delete Success !";
            return RedirectToAction("Index");
        }
    }
}