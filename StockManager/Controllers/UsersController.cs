﻿using System;
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
using StockManager.Helpers;

namespace StockManager.Controllers
{
    public class UsersController : Controller
    {
        private StockManagerEntities db = new StockManagerEntities();

        [Authorize(Roles = "Administrator")]
        public ActionResult Index(int? page)
        {
            var TenantId = Convert.ToInt32(Session["TenantID"]);
            var users = db.Users.Include(u => u.Tenant).Where(x => x.TenantId == TenantId).ToList();
            return View(users);
        }

        [Authorize(Roles = "Administrator")]
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

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(db.UserRoles, "Id", "RoleName");
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,UserName,Password,Email,Mobile,Phone,TenantId,RoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Password = Hash(user.Password);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(db.UserRoles, "Id", "RoleName");
            return View(user);
        }

        [Authorize(Roles = "Administrator")]
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
            ViewBag.RoleId = new SelectList(db.UserRoles, "Id", "RoleName");
            return View(user);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,Password,Email,Mobile,Phone,TenantId,RoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                if (db.SaveChanges() > 0 && Convert.ToInt32(Session["UserID"]) == user.UserId)
                {
                    var role = db.UserRoles.Find(user.RoleId);
                    Session["RoleName"] = role.RoleName;
                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(db.UserRoles, "Id", "RoleName");
            return View(user);
        }

        [Authorize(Roles = "Administrator")]
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

        [Authorize(Roles = "Administrator")]
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

            //string cookieName = FormsAuthentication.FormsCookieName;
            //HttpCookie authCookie = HttpContext.Request.Cookies.Get(cookieName);

            //if ( authCookie != null )
            //{
            //    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            //    string UserName = ticket.Name;
            //}            

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

                    User user = db.Users
                    .Where(x => x.UserName == login.username.Trim())
                    .FirstOrDefault();

                    if (user == null)
                        return HttpNotFound();

                    if (user.Password == password)
                    {
                        SignInRemember(login.username, user.UserRole.RoleName, login.remember_me);

                        Session["UserID"] = user.UserId;
                        Session["TenantID"] = user.TenantId;
                        Session["UserName"] = user.UserName;
                        //Session["FinancialYearID"] = user.Tenant.CurrentFinYear;
                        Session["RoleName"] = user.UserRole.RoleName;
                        if (user.UserCompanies.Count() == 0)
                        {
                            ViewBag.ErrorMsg = "You are not associated to any company. Please contact your administrator";
                            return View(login);
                        }
                        else
                        {
                            var company = user.UserCompanies.FirstOrDefault();

                            Session["CompanyID"] = company.CompanyId;
                            Session["FinancialYearID"] = company.Company.CurrentFinYear;
                            Session["YearStartDate"] = company.Company.FinancialYear.StartDate.ToString("MM/dd/yyyy");
                            Session["YearEndDate"] = company.Company.FinancialYear.EndDate.ToString("MM/dd/yyyy");
                            Session["CompanyName"] = company.Company.Name;

                        }
                        return RedirectToLocal(login.return_url);
                    }
                    else
                    {
                        //Login Fail
                        ViewBag.ErrorMsg = "Username/Password combination not found";
                        return View(login);
                    }
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMsg = ex.Message;
            }
            return View(login);
        }

        private void SignInRemember(string userName, string role, bool isPersistent = false)
        {
            // Clear any lingering authencation data
            FormsAuthentication.SignOut();

            FormsAuthentication.SetAuthCookie(userName, isPersistent);

            var authTicket = new FormsAuthenticationTicket(
            1,
            userName,
            DateTime.Now,
            DateTime.Now.AddDays(30),
            isPersistent,
            role,
            "/"
            );

            //encrypt the ticket and add it to a cookie
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));

            if ( authTicket.IsPersistent)
            {
                cookie.Expires = authTicket.Expiration;
            }

            Response.Cookies.Add(cookie);

            // Write the authentication cookie

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
                return RedirectToAction("Index", "dashboard");
            }
            catch
            {
                throw;
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string email)
        {

            if (!String.IsNullOrEmpty(email))
            {

                string token = Guid.NewGuid().ToString();

                var url = ResolveServerUrl(VirtualPathUtility.ToAbsolute("/Users/ResetPassword"), false)
                + "?token=" + token;
                var message = "<a href='" + url + "'>Click here to reset your password.</a>";

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
            if (!String.IsNullOrEmpty(password) && !String.IsNullOrEmpty(password))
            {
                if (password == confirm_password)
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

        [Authorize(Roles = "Administrator")]
        public JsonResult IsUserExists(string UserName)
        {
            return Json(!db.Users.Any(x => x.UserName == UserName), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrator")]
        public JsonResult IsEmailExists(string Email)
        {
            return Json(!db.Users.Any(x => x.Email == Email), JsonRequestBehavior.AllowGet);
        }

        private static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            if (serverUrl.IndexOf("://") > -1)
                return serverUrl;

            string newUrl = serverUrl;
            Uri originalUri = System.Web.HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) +
            "://" + originalUri.Authority + newUrl;
            return newUrl;
        }

        public ActionResult NoAccess()
        {
            return View();
        }

        [Authorize]
        public ActionResult profile()
        {
            var user_id = Session["UserID"] as int?;

            var user = db.Users.Find(user_id);

            return View(user);

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult profile(string btnUpdateProfile, string btnUpdateEmail, string btnUpdatePassword, User user)
        {

            var user_id = Session["UserID"] as int?;

            var db_user = db.Users.Find(user_id);

            if (db_user.Email != user.Email)
            {
                if (db.Users.Where(x => x.Email == user.Email).FirstOrDefault() == null)
                {
                    db_user.Email = user.Email;
                }
                else
                {
                    ModelState.AddModelError("User.Email", "This email address is already associated with a different account.");
                }
            }

            if (db_user.UserName != user.UserName)
            {
                if (db.Users.Where(x => x.UserName == user.UserName).FirstOrDefault() == null)
                {
                    db_user.UserName = user.UserName;
                }
                else
                {
                    ModelState.AddModelError("User.UserName", "This username is already associated with a different account.");
                }
            }


            if (ModelState.IsValid)
            {
                db_user.Mobile = user.Mobile;
                db_user.Phone = user.Phone;
                db.Entry(db_user).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Success"] = "Profile updated.";

                return RedirectToAction("profile");

            }

            return View(user);

        }

    }
}
