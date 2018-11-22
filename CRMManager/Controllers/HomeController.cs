using System.Web.Mvc;
using CRMManager.DataAccess;
using CRMManager.Models;


namespace CRMManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Please enter your email id and password to login to Athena CRM";
            return View();
        }

        public ActionResult UserLogout()
        {
            Session["UserId"] = null;
            Session["UserType"] = null;
            Session["UserName"] = null;
            ViewBag.Message = "You have signed out of Athena CRM";
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginActionSubmit(Authentication authentication)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Please enter your email id and password to login to Athena CRM";
                return View("Index");
            }

            Authentication returnAuthentication = new AuthenticationDao().LoginSearch(authentication);
            if (returnAuthentication?.UserType != null)
            {
                Session["UserId"] = returnAuthentication.LoginId;
                Session["UserType"] = returnAuthentication.UserType;
                Session["UserName"] = returnAuthentication.UserName;
                ViewBag.Message = "Please enter your email id and password to login to Athena CRM";
                return RedirectToAction("MemberSearch", "Members");
            }
            else
            {
                ViewBag.Message = "Sorry, unable to athentication with the given credentials";
            }

            return View("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}