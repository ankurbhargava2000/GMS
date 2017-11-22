using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StockManager.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using System.Security.Principal;
using PagedList;
using System.Net.Mail;
using StockManager.Helpers;

namespace StockManager.Controllers
{
    public class UsersController : Controller
    {
       

        private StockManagerEntities db = new StockManagerEntities();

        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string email)
        {

            if ( !String.IsNullOrEmpty(email) )
            {

                string token = Guid.NewGuid().ToString();

                var url = ResolveServerUrl(VirtualPathUtility.ToAbsolute("/Users/ResetPassword"), false)
                     + "?token=" + token;
                var message = "<a href='"+url+"'>Click here to reset your password.</a>";                

                var user = db.Users.Where(x => x.Email == email).FirstOrDefault();
                user.password_reset_token = token;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                GMailer.Send(email, "Password reset link", message);

            }

            return View();
        }

        public ActionResult ResetPassword(string token)
        {
            ViewBag.token = token;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string token, string password, string confirm_password)
        {
            if ( !String.IsNullOrEmpty(password) && !String.IsNullOrEmpty(password) )
            {
                if ( password == confirm_password )
                {
                    var user = db.Users.Where(x => x.password_reset_token == token).FirstOrDefault();
                    user.Password = Hash(password);
                    user.password_reset_token = String.Empty;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    Session["SUCCESS"] = "Please login with your new password";
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.ERR = "Password & Confirm password do not match";
                }
            }
            else
            {
                ViewBag.ERR = "Please type password and confirm password fields";
            }
            
            ViewBag.token = token;
            return View();

        }

        [CheckAuth]
        public ActionResult Index(int? page)
        {
            var users = db.Users.Include(u => u.Tenant).OrderBy(x => x.UserId);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
        }

        [CheckAuth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [CheckAuth]
        public ActionResult Create()
        {
            ViewBag.TenantId = new SelectList(db.Tenants, "Id", "Name");
            return View();
        }

        [CheckAuth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,UserName,Password,Email,Mobile,Phone,TenantId")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Password = Hash(user.Password);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TenantId = new SelectList(db.Tenants, "Id", "Name", user.TenantId);
            return View(user);
        }

        [CheckAuth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.TenantId = new SelectList(db.Tenants, "Id", "Name", user.TenantId);
            return View(user);
        }

        [CheckAuth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,Password,Email,Mobile,Phone,TenantId")] User user)
        {
            if (ModelState.IsValid)
            {

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TenantId = new SelectList(db.Tenants, "Id", "Name", user.TenantId);
            return View(user);
        }

        [CheckAuth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [CheckAuth]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Login(string returnURL)
        {
            return View(new LoginVM() { return_url = returnURL });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM login)
        {            
            try
            {
                if (ModelState.IsValid)
                {
                    string password = Hash(login.password.Trim());

                    User user = db.Users.Where(x => x.UserName == login.username.Trim()).FirstOrDefault();

                    if (user == null)
                        return HttpNotFound();

                    if (user.Password == password)
                    {
                        SignInRemember(login.username, true);

                        Session["UserID"] = user.UserId;
                        Session["TenantID"] = user.TenantId;
                        Session["FinancialYearID"] = user.Tenant.CurrentFinYear;

                        return RedirectToLocal(login.return_url);

                    }
                    else
                    {
                        //Login Fail
                        TempData["ErrorMSG"] = "Access Denied! Wrong Credential";
                        return View(login);
                    }

                }

            }
            catch (Exception e)
            {

            }
            
            return View(login);

        }
        
        private void SignInRemember(string userName, bool isPersistent = false)
        {
            // Clear any lingering authencation data
            FormsAuthentication.SignOut();

            // Write the authentication cookie
            FormsAuthentication.SetAuthCookie(userName, isPersistent);
        }

        private void EnsureLoggedOut()
        {
            // If the request is (still) marked as authenticated we send the user to the logout action
            if (Request.IsAuthenticated)
                Logout();
        }
        
        public ActionResult Logout()
        {
            try
            {
                // First we clean the authentication ticket like always
                //required NameSpace: using System.Web.Security;
                FormsAuthentication.SignOut();

                // Second we clear the principal to ensure the user does not retain any authentication
                //required NameSpace: using System.Security.Principal;
                HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

                Session.Clear();
                System.Web.HttpContext.Current.Session.RemoveAll();

                // Last we redirect to a controller/action that requires authentication to ensure a redirect takes place
                // this clears the Request.IsAuthenticated flag since this triggers a new request
                return RedirectToLocal();
            }
            catch
            {
                throw;
            }
        }
        private ActionResult RedirectToLocal(string returnURL = "")
        {
            try
            {
                // If the return url starts with a slash "/" we assume it belongs to our site
                // so we will redirect to this "action"
                if (!string.IsNullOrWhiteSpace(returnURL) && Url.IsLocalUrl(returnURL))
                    return Redirect(returnURL);

                // If we cannot verify if the url is local to our host we redirect to a default location
                return RedirectToAction("Index", "PurchaseOrders");
            }
            catch
            {
                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private string Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public JsonResult IsUserExists(string UserName)
        {
            return Json(!db.Users.Any(x => x.UserName == UserName), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsEmailExists(string Email)
        {
            return Json(!db.Users.Any(x => x.Email == Email), JsonRequestBehavior.AllowGet);
        }

        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            if (serverUrl.IndexOf("://") > -1)
                return serverUrl;

            string newUrl = serverUrl;
            Uri originalUri = System.Web.HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) +
                "://" + originalUri.Authority + newUrl;
            return newUrl;
        }

    }
}
