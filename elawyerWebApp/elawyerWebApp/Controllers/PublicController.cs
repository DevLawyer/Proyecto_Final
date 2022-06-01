using elawyerWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace elawyerWebApp.Controllers
{
    public class PublicController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Services()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult LogIn(string username, string password)
        {
            String message = "";
            try
            {
                using (elawyerdbEntities db = new elawyerdbEntities())
                {
                    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                    {
                        User user = db.User.FirstOrDefault(u => u.Username == username && u.Password == password);

                        if (user != null)
                        {
                            FormsAuthentication.SetAuthCookie(user.Username, false);
                            Role role = db.Role.FirstOrDefault(r => r.No == user.NoRole);
                            Session["user"] = user;
                            Session["role"] = role;
                            return RedirectToAction("Dashboard", "Private");
                        }
                        else
                        {
                            message = "El usuario o la contraseña son incorrectos.";
                        }
                    }
                    else
                    {
                        if (Session["firstLogin"] != null && (Boolean)Session["firstLogin"])
                        {
                            message = "Debe indicar un usuario y contraseña.";
                        }
                        else
                        {
                            message = "";
                            Session["firstLogin"] = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            ViewBag.Message = message;
            return View();
        }
    }
}