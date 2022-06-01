using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using elawyerWebApp.Models;
using elawyerWebApp.Models.ViewModels;

namespace elawyerWebApp.Controllers
{
    [Authorize]
    public class PrivateController : Controller
    {
        private User user;

        // GET: Private
        public ActionResult Dashboard()
        {
            user = (User)Session["user"];
            int usersCount = 0;
            int clientsCount = 0;
            int payInvCount = 0;
            int noPayInvCount = 0;
            int issueCount = 0;
            double totAmount = 0;
            double totAmountPay = 0;
            double totAmountNoPay = 0;
            using (elawyerdbEntities db = new elawyerdbEntities())
            {
                if (user.NoRole == 2)
                {
                    clientsCount = (from c in db.Client
                                   where c.NoUser == user.No
                                   select c).Count();
                    issueCount = (from i in db.Issue
                                  from c in db.Client
                                  where c.NoUser == user.No
                                  where i.NoClient == c.No
                                  select i).Count();
                    totAmount = (double)(from i in db.InvoiceHeader
                                         from iss in db.Issue
                                         from c in db.Client
                                         where c.NoUser == user.No
                                         where iss.NoClient == c.No
                                         where i.NoIssue == iss.No
                                         select i.TotalAmount).Sum();
                }
                else
                {
                    usersCount = db.User.Count();
                    clientsCount = db.Client.Count();
                    payInvCount = db.InvoiceHeader.Count(ih => ih.NoStatus == 1);
                    noPayInvCount = db.InvoiceHeader.Count(ih => ih.NoStatus == 2);
                    issueCount = db.Issue.Count();
                    totAmount = (double)db.InvoiceHeader.Sum(i => i.TotalAmount);
                    totAmountPay = (double)db.InvoiceHeader.Where(i => i.NoStatus == 1).Sum(i => i.TotalAmount);
                    totAmountNoPay = (double)db.InvoiceHeader.Where(i => i.NoStatus == 2).Sum(i => i.TotalAmount);
                }
            }



            ViewData["usersCount"] = usersCount;
            ViewData["clientsCount"] = clientsCount;
            ViewData["payInvCount"] = payInvCount;
            ViewData["noPayInvCount"] = noPayInvCount;
            ViewData["issueCount"] = issueCount;
            ViewData["totAmount"] = totAmount;
            ViewData["totAmountPay"] = totAmountPay;
            ViewData["totAmountNoPay"] = totAmountNoPay;
            return View();
        }

        // ADMIN VIEWS - START
        public ActionResult ListUser()
        {
            user = (User)Session["user"];
            if (user.NoRole != 1)
            {
                return RedirectToAction("Dashboard", "Private");
            }
            else
            {
                return View();
            }
        }

        public ActionResult CardUser()
        {
            user = (User)Session["user"];
            User currUser;
            Role currRole;
            int no;
            bool show = int.TryParse(Request.QueryString["user"], out no);
            if (user.NoRole != 1)
            {
                return RedirectToAction("Dashboard", "Private");
            }
            else
            {
                if (show) { 
                    using (elawyerdbEntities db = new elawyerdbEntities())
                    {
                        currUser = (User)(from u in db.User
                                          where u.No == no
                                          select u).FirstOrDefault();
                        currRole = (Role)(from r in db.Role
                                          where r.No == currUser.NoRole
                                          select r).FirstOrDefault();
                    }
                    ViewData["currUser"] = currUser;
                    ViewData["currRole"] = currRole;
                }
                return View();
            }
        }
        [HttpPost]
        public ActionResult NewUser(UserVM newUser)
        {
            try
            {
                user = (User)Session["user"];
                if (user.NoRole != 1)
                    return RedirectToAction("Dashboard", "Private");

                if (ModelState.IsValid)
                {
                    using (elawyerdbEntities db = new elawyerdbEntities())
                    {
                        User oUser = new User();
                        oUser.Name = newUser.Name;
                        oUser.Surname = newUser.Surname;
                        oUser.NIF = newUser.NIF;
                        oUser.Telephone = newUser.Telephone;
                        oUser.Email = newUser.Email;
                        oUser.Username = newUser.Username;
                        oUser.Password = newUser.Password;
                        oUser.NoRole = newUser.NoRole;

                        db.User.Add(oUser);
                        db.SaveChanges();
                    }

                    return RedirectToAction("ListUser", "Private");
                }

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("CardUser", "Private");
        }
        public ActionResult DeleteUser()
        {
            user = (User)Session["user"];
            if (user.NoRole != 1)
                return RedirectToAction("Dashboard", "Private");
            int no;
            bool show = int.TryParse(Request.QueryString["user"], out no);
            if (show)
            {
                using (elawyerdbEntities db = new elawyerdbEntities())
                {
                    User delUser = db.User.Find(no);
                    db.User.Remove(delUser);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("ListUser", "Private");
        }
        // ADMIN VIEWS - END


        // USER VIEW - START
        public ActionResult MyProfile()
        {
            return View();
        }
        // USER VIEW - END

        // ISSUES VIEWS - START
        public ActionResult ListIssue()
        {
            user = (User)Session["user"];
            List<Issue> listIssue;
            List<Status> listStatus;
            using (elawyerdbEntities db = new elawyerdbEntities())
            {
                listStatus = db.Status.ToList();

                if (user.NoRole == 2)
                {
                    listIssue = (from i in db.Issue
                                 from c in db.Client
                                 where c.NoUser == user.No
                                 where i.NoClient == c.No
                                 select i).ToList();
                }
                else
                    listIssue = db.Issue.ToList();
            }
            ViewData["listIssue"] = listIssue;
            ViewData["listStatus"] = listStatus;
            return View();
        }
        
        public ActionResult CardIssue()
        {
            user = (User)Session["user"];
            Issue currIssue;
            Client currClient = null;
            List<Status> listStatus;
            int no;
            bool show = int.TryParse(Request.QueryString["issue"], out no);
            if (show)
            {
                using (elawyerdbEntities db = new elawyerdbEntities())
                {
                    listStatus = db.Status.ToList();
                    currIssue = db.Issue.FirstOrDefault(i => i.No == no);
                    if (currIssue != null)
                    {
                        currClient = db.Client.FirstOrDefault(c => c.No == currIssue.NoClient);
                    }
                }
                ViewData["currIssue"] = currIssue;
                ViewData["currCliente"] = currClient;
                ViewData["listStatus"] = listStatus;
            }
            return View();
        }
        [HttpPost]
        public ActionResult NewIssue(IssueVM newIssue)
        {            
            try
            {
                user = (User)Session["user"];
                if (user.NoRole == 3)
                    return RedirectToAction("ListIssue", "Private");

                if (ModelState.IsValid)
                {
                    using (elawyerdbEntities db = new elawyerdbEntities()) 
                    {
                        Issue oIssue = new Issue();
                        oIssue.NoRef = newIssue.NoRef;
                        oIssue.Description = newIssue.Description;
                        oIssue.CreationDate = newIssue.CreationDate;
                        oIssue.StartDate = newIssue.StartDate;
                        oIssue.EndDate = newIssue.EndDate;
                        oIssue.Hours = newIssue.Hours;
                        oIssue.Fee = newIssue.Fee;
                        oIssue.NoClient = newIssue.NoClient;
                        oIssue.NoStatus = newIssue.NoStatus;

                        db.Issue.Add(oIssue);
                        db.SaveChanges();
                    }

                    return RedirectToAction("ListIssue", "Private");
                }
                
            }
            catch(Exception ex)
            {

            }
            return RedirectToAction("CardIssue", "Private");
        }

        public ActionResult DeleteIssue()
        {
            user = (User)Session["user"];
            if (user.NoRole == 3)
                return RedirectToAction("Dashboard", "Private");
            int no;
            bool show = int.TryParse(Request.QueryString["issue"], out no);

            if (show)
            {
                using (elawyerdbEntities db = new elawyerdbEntities())
                {
                    Issue delIssue = db.Issue.Find(no);
                    List<InvoiceHeader> delInvoices = db.InvoiceHeader.Where(i => i.NoIssue == delIssue.No).ToList();
                    
                    foreach (InvoiceHeader ih in delInvoices)
                    {
                        db.InvoiceLine.RemoveRange(db.InvoiceLine.Where(il => il.NoInvoiceHeader == ih.No));
                        db.InvoiceHeader.Remove(ih);
                    }

                    db.Issue.Remove(delIssue);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("ListIssue", "Private");
        }
        // ISSUES VIEWS - END

        // INVOICES VIEWS - START
        public ActionResult ListInvoice()
        {
            user = (User)Session["user"];
            List<InvoiceHeader> listInvoice;
            List<InvoiceStatus> listStatus;
            int no;
            string show = Request.QueryString["status"];
            using (elawyerdbEntities db = new elawyerdbEntities())
            {
                listStatus = db.InvoiceStatus.ToList();

                if (user.NoRole == 2)
                {
                    if (show != "" && show != null)
                    {
                        if (show.Equals("pay"))
                        {
                            listInvoice = (from i in db.InvoiceHeader
                                           from iss in db.Issue
                                           from c in db.Client
                                           where c.NoUser == user.No
                                           where iss.NoClient == c.No
                                           where i.NoIssue == iss.No
                                           where i.NoStatus == 1
                                           select i).ToList();
                        }
                        else
                        {
                            listInvoice = (from i in db.InvoiceHeader
                                           from iss in db.Issue
                                           from c in db.Client
                                           where c.NoUser == user.No
                                           where iss.NoClient == c.No
                                           where i.NoIssue == iss.No
                                           where i.NoStatus == 2
                                           select i).ToList();
                        }
                    }
                    else
                    {
                        listInvoice = (from i in db.InvoiceHeader
                                       from iss in db.Issue
                                       from c in db.Client
                                       where c.NoUser == user.No
                                       where iss.NoClient == c.No
                                       where i.NoIssue == iss.No
                                       select i).ToList();
                    }
                }
                else
                {
                    if (show != "" && show != null)
                    {
                        if (show.Equals("pay"))
                        {
                            listInvoice = (from i in db.InvoiceHeader
                                           where i.NoStatus == 1
                                           select i).ToList();
                        }
                        else 
                        { 
                            listInvoice = (from i in db.InvoiceHeader
                                           where i.NoStatus == 2
                                           select i).ToList();
                        }
                    }
                    else
                        listInvoice = db.InvoiceHeader.ToList();
                }
            }
            ViewData["listInvoice"] = listInvoice;
            ViewData["listInvStatus"] = listStatus;
            return View();
        }
        public ActionResult CardInvoice()
        {
            InvoiceHeader currInvoice = null;
            List<InvoiceLine> currInvLines;
            List<InvoiceStatus> listStatus;
            int no;
            bool show = int.TryParse(Request.QueryString["invoice"], out no);
            if (show)
            {
                using (elawyerdbEntities db = new elawyerdbEntities())
                {
                    listStatus = db.InvoiceStatus.ToList();
                    currInvoice = db.InvoiceHeader.FirstOrDefault(i => i.No == no);
                    currInvLines = db.InvoiceLine.Where(il => il.NoInvoiceHeader == currInvoice.No).ToList();
                }
                ViewData["currInvoice"] = currInvoice;
                ViewData["currInvLines"] = currInvLines;
                ViewData["status"] = listStatus;
            }
            return View();
        }       
        [HttpPost]
        public ActionResult NewInvoiceHeader(InvoiceGral newInvoiceH)
        {
            return RedirectToAction("Index", "Public");
            try
            {
                user = (User)Session["user"];
                if (user.NoRole == 2)
                    return RedirectToAction("ListInvoice", "Private");
                InvoiceHeader currInvoice;
                List<InvoiceLine> currInvLines;
                List<InvoiceStatus> listStatus;
                if (ModelState.IsValid)
                {
                    InvoiceHeader oInvoice = new InvoiceHeader();
                    using (elawyerdbEntities db = new elawyerdbEntities())
                    {
                        oInvoice.CreateDate = newInvoiceH.InvHeader.CreateDate;
                        oInvoice.PaymentDate = newInvoiceH.InvHeader.PaymentDate;
                        oInvoice.NoIssue = newInvoiceH.InvHeader.NoIssue;
                        oInvoice.NoStatus = newInvoiceH.InvHeader.NoStatus;
                        oInvoice.NoUser = user.No;

                        db.InvoiceHeader.Add(oInvoice);
                        db.SaveChanges();

                        listStatus = db.InvoiceStatus.ToList();
                        currInvoice = db.InvoiceHeader.FirstOrDefault(i => i.No == oInvoice.No);
                        currInvLines = db.InvoiceLine.Where(il => il.NoInvoiceHeader == currInvoice.No).ToList();
                    }
                    ViewData["currInvoice"] = currInvoice;
                    ViewData["currInvLines"] = currInvLines;
                    ViewData["status"] = listStatus;

                    return RedirectToAction("CardInvoice", "Private", new { invoice=oInvoice.No});
                }

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("CardInvoice", "Private");
        }
        [HttpPost]
        public ActionResult NewInvoiceLine(FormCollection values)
        {
            return RedirectToAction("Index", "Public");
            try
            {
                user = (User)Session["user"];
                if (user.NoRole == 2)
                    return RedirectToAction("ListInvoice", "Private");
                int no;
                bool show = int.TryParse(values["invHeader"], out no);
                if (show && no!= 0)
                {
                    if (ModelState.IsValid)
                    {
                        InvoiceLine oInvoice = new InvoiceLine();
                        using (elawyerdbEntities db = new elawyerdbEntities())
                        {
                            oInvoice.Concept = values["invConcept"];
                            decimal amount = 0;
                            if (Decimal.TryParse(values["invAmount"], out amount)) { };
                            oInvoice.Amount = amount;
                            oInvoice.Amount = no;
                            db.InvoiceLine.Add(oInvoice);
                            db.SaveChanges();

                        }

                        return RedirectToAction("CardInvoice", "Private",new { invoice=no});
                    }
                    return RedirectToAction("ListInvoice", "Private");
                }
            }
            catch (Exception ex)
            {
            }
            return RedirectToAction("Dashboard", "Private");
        }
        public ActionResult DeleteInvoiceHeader()
        {
            user = (User)Session["user"];
            if (user.NoRole == 2)
                return RedirectToAction("Dashboard", "Private");

            int no;
            bool show = int.TryParse(Request.QueryString["invoiceHeader"], out no);

            if (show)
            {
                using (elawyerdbEntities db = new elawyerdbEntities())
                {
                    InvoiceHeader delInvoice = db.InvoiceHeader.FirstOrDefault(i => i.No == no);
                    db.InvoiceLine.RemoveRange(db.InvoiceLine.Where(il => il.NoInvoiceHeader == delInvoice.No));
                    db.InvoiceHeader.Remove(delInvoice);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("ListInvoice", "Private");
        }
        public ActionResult DeleteInvoiceLine()
        {
            user = (User)Session["user"];
            if (user.NoRole == 2)
                return RedirectToAction("Dashboard", "Private");
            
            int noInv = 0;
            int no;
            bool show = int.TryParse(Request.QueryString["invoiceLine"], out no);

            if (show)
            {
                using (elawyerdbEntities db = new elawyerdbEntities())
                {
                    InvoiceLine delInvoiceLine = db.InvoiceLine.FirstOrDefault(i => i.No == no);
                    noInv = delInvoiceLine.NoInvoiceHeader;
                    db.InvoiceLine.Remove(delInvoiceLine);
                    db.SaveChanges();
                }
            }

            if (noInv == 0) 
            {
                return RedirectToAction("CardInvoice", "Private");
            }
            else
            {
                return RedirectToAction("CardInvoice", "Private", new { invoice = noInv });
            }
        }
        // INVOICES VIEWS - END

        // CLIENTS VIEWS - START
        public ActionResult ListClient()
        {
            user = (User)Session["user"];
            List<Client> listClient;
            using (elawyerdbEntities db = new elawyerdbEntities())
            {
                if (user.NoRole == 2)
                {
                    listClient = (from c in db.Client
                                  where c.NoUser == user.No
                                  select c).ToList();
                }
                else
                {
                    listClient = db.Client.ToList();
                }
            }
            ViewData["listClient"] = listClient;
            return View();
        }
        public ActionResult CardClient()
        {
            Client currClient;
            int no;
            bool show = int.TryParse(Request.QueryString["client"], out no);
            if (show)
            {
                using (elawyerdbEntities db = new elawyerdbEntities())
                {
                    currClient = db.Client.FirstOrDefault(c => c.No == no);
                }
                ViewData["currCliente"] = currClient;
            }
            return View();
        }
        [HttpPost]
        public ActionResult NewClient(ClientVM newClient)
        {
            try
            {
                user = (User)Session["user"];
                if (user.NoRole == 2)
                    return RedirectToAction("ListClient", "Private");

                if (ModelState.IsValid)
                {
                    using (elawyerdbEntities db = new elawyerdbEntities())
                    {
                        Client oClient = new Client();
                        oClient.Name = newClient.Name;
                        oClient.Surname = newClient.Surname;
                        oClient.NIF = newClient.NIF;
                        oClient.Address1 = newClient.Address1;
                        oClient.Address2 = newClient.Address2;
                        oClient.Telephone = newClient.Telephone;
                        oClient.Email = newClient.Email;
                        oClient.NoUser = newClient.NoUser;

                        db.Client.Add(oClient);
                        db.SaveChanges();
                    }

                    return RedirectToAction("ListClient", "Private");
                }

            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("CardClient", "Private");
        }
        public ActionResult DeleteClient()
        {
            user = (User)Session["user"];
            if (user.NoRole == 2)
                return RedirectToAction("Dashboard", "Private");
            int no;
            bool show = int.TryParse(Request.QueryString["client"], out no);
            if (show)
            {
                using (elawyerdbEntities db = new elawyerdbEntities())
                {
                    Client delClient = db.Client.Find(no);
                    db.Client.Remove(delClient);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("ListClient", "Private");
        }
        // CLIENTS VIEWS - END

        // LOG OUT VIEW - START
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Public");
        }
        // LOG OUT VIEW - END
    }
}