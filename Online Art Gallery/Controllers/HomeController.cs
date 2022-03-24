using Online_Art_Gallery.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Online_Art_Gallery.Controllers
{
    public class HomeController : Controller
    {
        ArtGalleryEntities entities = new ArtGalleryEntities();
        /* -----------------------------------
             Home Page
        --------------------------------------*/

        // GET: Home
        public ActionResult Index()
        {
            ViewData["Banner"] = entities.Banners.ToList();
            ViewData["Category"] = entities.Categories.ToList();

            var artworks = entities.Artworks.Where(x => x.Status == true).OrderByDescending(x => x.Id).Take(9).ToList();
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
            ViewData["wishlists"] = entities.Wishlists.ToList();
            return View();
        }
        // GET: HomeUser/About
        public ActionResult About()
        {
            return View();
        }


        /* -----------------------------------
             Login - Register - Logout
        --------------------------------------*/

        // GET: HomeUser/Login
        public ActionResult Login()
        {
            return View();
        }
        // POST: HomeUser/Login
        [HttpPost]
        public ActionResult Login(string name, string password)
        {
            if (name == "")
            {
                TempData["name-validation"] = "Please Enter Name..!";
                return View();
            }
            if (password == "")
            {
                TempData["password-validation"] = "Please Enter Password..!";
                return View();
            }
            var f_password = GetMD5(password);
            var data = entities.Users.Where(s => s.Login_Name.Equals(name) && s.Login_Password.Equals(f_password)).ToList();
            if (data.Count() > 0)
            {
                Session["Id"] = data.FirstOrDefault().Id;
                Session["Name"] = data.FirstOrDefault().Name;
                TempData["Success"] = "Login Success !";
                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.error = "Login failed";
                return View();
            }
        }

        // GET: HomeUser/Register
        public ActionResult Register()
        {
            return View();
        }
        // POSt: HomeUser/Register
        [HttpPost]
        public ActionResult Register(string name, string email, string phone, string address, DateTime? birth_date, bool? gender, string login_name, string password, string c_password)
        {

            if (name == "")
            {
                TempData["name-validation"] = "Please Enter Name..!";
                return View();
            }
            if (email == "")
            {
                TempData["email-validation"] = "Please Enter Email..!";
                return View();
            }
            else
            {
                string regexPattern = @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}";
                if (!System.Text.RegularExpressions.Regex.IsMatch(email, regexPattern))
                {
                    TempData["email-validation"] = "Please enter the correct email format..!";
                    return View();
                }

            }
            if (phone == "")
            {
                TempData["phone-validation"] = "Please Enter Phone..!";
                return View();
            }
            if (address == "")
            {
                TempData["address-validation"] = "Please Enter Address..!";
                return View();
            }
            if (birth_date == null)
            {
                TempData["birth_date-validation"] = "Please Enter Birth Date..!";
                return View();
            }
            if (login_name == "")
            {
                TempData["login_name-validation"] = "Please Enter Account Name..!";
                return View();
            }
            if (password == "")
            {
                TempData["password-validation"] = "Please Enter Password..!";
                return View();
            }
            if (c_password == "")
            {
                TempData["c_password-validation"] = "Please Enter Confirm Password..!";
                return View();
            }
            if (c_password != password)
            {
                TempData["c_password-validation"] = "Password does not match..!";
                return View();
            }

            DateTime date = DateTime.Now;
            DateTime date_s = Convert.ToDateTime(birth_date);
            int Time = date.Year - date_s.Year;
            if (Time < 18)
            {
                TempData["Error"] = "You are under 18 years old to register..!";
                return View();
            }

            User user = new User();
            var check_email = entities.Users.FirstOrDefault(s => s.Email == email);
            var check_phone = entities.Users.FirstOrDefault(s => s.Phone == phone);
            var check_login_name = entities.Users.FirstOrDefault(s => s.Login_Name == login_name);

            if (check_email == null)
            {
                if (check_phone == null)
                {
                    if (check_login_name == null)
                    {
                        user.Name = name;
                        user.Email = email;
                        user.Phone = phone;
                        user.Address = address;
                        user.BirthDate = birth_date;
                        user.Gender = gender;
                        user.Login_Name = login_name;
                        user.Login_Password = GetMD5(password);
                        user.Status = false;

                        if (gender == true)
                        {
                            user.Picture = "male.jpg";
                        }
                        else
                        {
                            user.Picture = "female.jpg";
                        }

                        entities.Users.Add(user);
                        entities.SaveChanges();
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        TempData["login_name-validation"] = "Account Name already exists..!";
                        return View();
                    }
                }
                else
                {
                    TempData["phone-validation"] = "Phone already exists..!";
                    return View();
                }
            }
            else
            {
                TempData["email-validation"] = "Email already exists..!";
                return View();
            }
        }

        // GET: HomeUser/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        /* -----------------------------------
             Profile
        --------------------------------------*/

        // GET: HomeUser/Profile
        public ActionResult Profile(int? id)
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.User = entities.Users.Find(id);
            return View();
        }
        // POST: HomeUser/UpdateProfile
        [HttpPost]
        public ActionResult UpdateProfile(int id, string name, HttpPostedFileBase picture, string email, string phone, string address, DateTime? birth_date, bool? gender)
        {
            if (name == "")
            {
                TempData["name-validation"] = "Please Enter Name..!";
                return RedirectToAction("Profile", new { id = id });
            }
            if (email == "")
            {
                TempData["email-validation"] = "Please Enter Email..!";
                return RedirectToAction("Profile", new { id = id });
            }
            else
            {
                string regexPattern = @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}";
                if (!System.Text.RegularExpressions.Regex.IsMatch(email, regexPattern))
                {
                    TempData["email-validation"] = "Please enter the correct email format..!";
                    return RedirectToAction("Profile", new { id = id });
                }

            }
            if (phone == "")
            {
                TempData["phone-validation"] = "Please Enter Phone..!";
                return RedirectToAction("Profile", new { id = id });
            }
            if (address == "")
            {
                TempData["address-validation"] = "Please Enter Address..!";
                return RedirectToAction("Profile", new { id = id });
            }
            if (birth_date == null)
            {
                TempData["birth_date-validation"] = "Please Enter Birth Date..!";
                return RedirectToAction("Profile", new { id = id });
            }

            User user = entities.Users.Find(id);
            user.Name = name;

            if (picture != null)
            {
                var filename = picture.FileName;
                if (filename.ToLower().EndsWith("jpg") || filename.ToLower().EndsWith("png"))
                {
                    if (!Directory.Exists(Server.MapPath("~/Content/Images/User")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Images/User"));
                        picture.SaveAs(Server.MapPath("~/Content/Images/User/" + filename));
                    }
                    else
                    {
                        picture.SaveAs(Server.MapPath("~/Content/Images/User/" + filename));
                    }
                }
                else
                {
                    TempData["ErrorImage"] = "Photos must be of the correct type: jpg, png";
                    return RedirectToAction("Profile", new { id = id });
                }
                user.Picture = filename;
            }
            else
            {
                if (user.Picture == "male.jpg" || user.Picture == "female.jpg")
                {
                    if (gender == true)
                    {
                        user.Picture = "male.jpg";
                    }
                    else
                    {
                        user.Picture = "female.jpg";
                    }
                }
            }

            user.Email = email;
            user.Phone = phone;
            user.Address = address;
            user.BirthDate = birth_date;
            user.Gender = gender;

            entities.SaveChanges();

            TempData["Success"] = "Update Profile Success..!";
            return RedirectToAction("Profile", new { id = id });
        }

        // POST: HomeUser/ChangePassword
        [HttpPost]
        public ActionResult ChangePassword(int id, string old_password, string new_password, string confirm_password)
        {
            if (old_password == "")
            {
                TempData["old_password-validation"] = "Please Enter Your Current Password..!";
                return RedirectToAction("Profile", new { id = id });
            }
            if (new_password == "")
            {
                TempData["new_password-validation"] = "Please Enter New Password..!";
                return RedirectToAction("Profile", new { id = id });
            }
            if (confirm_password == "")
            {
                TempData["confirm_password-validation"] = "Please Enter Confirm Password..!";
                return RedirectToAction("Profile", new { id = id });
            }
            if (confirm_password != new_password)
            {
                TempData["confirm_password-validation"] = "Password does not match..!";
                return RedirectToAction("Profile", new { id = id });
            }
            User user = entities.Users.Find(id);
            var f_password = GetMD5(old_password);
            if (user.Login_Password == f_password)
            {
                if (new_password == confirm_password)
                {
                    user.Login_Password = GetMD5(new_password);
                    entities.SaveChanges();
                    TempData["Success"] = "Change Password Success..!";
                    return RedirectToAction("Profile", new { id = id });
                }
            }
            else
            {
                TempData["old_password-validation"] = "Your Current Password does not match..!";
                return RedirectToAction("Profile", new { id = id });
            }
            return RedirectToAction("Profile", new { id = id });
        }




        /* -----------------------------------
             ForgotPassword
        --------------------------------------*/

        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            if (email != "")
            {
                var users = entities.Users.FirstOrDefault(s => s.Email == email);

                if (users != null)
                {
                    var token = new StringBuilder();

                    RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
                    var data = new byte[4];
                    for (int i = 0; i < 10; i++)
                    {
                        //filled with an array of random numbers
                        rngCsp.GetBytes(data);
                        //this is converted into a character from A to Z
                        var randomchar = Convert.ToChar(
                                                  //produce a random number 
                                                  //between 0 and 25
                                                  BitConverter.ToUInt32(data, 0) % 26
                                                  //Convert.ToInt32('A')==65
                                                  + 65
                                         );
                        token.Append(randomchar);
                    }

                    MailMessage mailmessage = new MailMessage();
                    mailmessage.To.Add(email);
                    mailmessage.Subject = "Forgot Password";
                    mailmessage.Body = "Code: " + token;
                    mailmessage.From = new MailAddress(email);
                    mailmessage.IsBodyHtml = false;

                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = true;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new System.Net.NetworkCredential("chihiro2k1@gmail.com", "012202784a");
                    smtp.Send(mailmessage);


                    users.Code_Password = token.ToString();
                    entities.SaveChanges();

                    Session["code"] = users.Code_Password;

                    TempData["Success"] = "Code has been sent to your email successfully...!!";
                    return RedirectToAction("ForgotPasswordCode");
                }
                else
                {
                    TempData["Error"] = "Email does not exist...!!";
                }
            }
            else
            {
                TempData["EmailNull"] = "Please enter Email...!!";
                return View();
            }
            return View();
        }


        public ActionResult ForgotPasswordCode()
        {
            if (Session["code"] != null)
            {
                return View();

            }
            else
            {
                TempData["EmailNull"] = "Please enter your account registration email...!!";
                return RedirectToAction("ForgotPassword");
            }

        }
        [HttpPost]
        public ActionResult ForgotPasswordCode(string code)
        {
            if (code != "")
            {
                var users = entities.Users.FirstOrDefault(s => s.Code_Password == code);
                if (users != null)
                {
                    var token_reset = Guid.NewGuid().ToString();
                    Session["token_reset"] = token_reset;
                    TempData["Success"] = "Code matches, you can reset your password now...!!";
                    return RedirectToAction("ResetPassword");
                }
                else
                {
                    TempData["Error"] = "Code does not match...!!";
                    return View();
                }
            }
            else
            {
                TempData["CodeNull"] = "Please enter Code...!!";
                return View();
            }
        }

        public ActionResult ResetPassword()
        {
            if (Session["code"] != null)
            {
                if (Session["token_reset"] != null)
                {
                    return View();
                }
                else
                {
                    TempData["CodeNull"] = "Please enter your Code...!!";
                    return RedirectToAction("ForgotPasswordCode");
                }
            }
            else
            {
                TempData["EmailNull"] = "Please enter your account registration email...!!";
                return RedirectToAction("ForgotPassword");
            }
        }
        [HttpPost]
        public ActionResult ResetPassword(string new_pass, string cf_pass)
        {
            if (new_pass == "")
            {
                TempData["new_pass"] = "Please enter New Password...!!";
                return View();
            }
            if (cf_pass == "")
            {
                TempData["cf_pass"] = "Please enter Confirm Password...!!";
                return View();
            }
            if (Session["code"] == null)
            {
                TempData["EmailNull"] = "Please enter your account registration email...!!";
                return RedirectToAction("ForgotPassword");
            }

            string code = Session["code"].ToString();

            var user = entities.Users.FirstOrDefault(s => s.Code_Password == code);
            if (user != null)
            {
                if (new_pass == cf_pass)
                {
                    user.Login_Password = GetMD5(new_pass);
                    user.Code_Password = "";
                    entities.SaveChanges();
                    Session.Clear();
                    TempData["Success"] = "Password reset successful...!!";
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    TempData["cf_pass"] = "Password does not match..!!";
                    return View();
                }
            }
            return View();
        }


        // Password Encryption
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