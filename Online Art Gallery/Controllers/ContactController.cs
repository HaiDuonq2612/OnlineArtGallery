using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Online_Art_Gallery.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }
        // POST: Contact/SendEmail
        [HttpPost]
        public ActionResult SendEmail(string name, string email, string message)
        {
            //Validation Data
            if (name == "")
            {
                TempData["name-validation"] = "Please Enter Name..!";
                return RedirectToAction("Index");
            }
            if (email == "")
            {
                TempData["email-validation"] = "Please Enter Email..!";
                return RedirectToAction("Index");
            }
            if (message == "")
            {
                TempData["message-validation"] = "Please Enter Message..!";
                return RedirectToAction("Index");
            }

            string email_admin = "haiduong261201@gmail.com";
            MailMessage  mailmessage = new MailMessage();
            mailmessage.To.Add(email_admin);
            mailmessage.Subject = name + " - " +email;
            mailmessage.Body = message;
            mailmessage.From = new MailAddress(email_admin);
            mailmessage.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.UseDefaultCredentials = true;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("chihiro2k1@gmail.com", "012202784a");
            smtp.Send(mailmessage);

            TempData["Success"] = "The mail been sent to Successfully...!";
            return RedirectToAction("Index");
        }
    }
}