using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using CRMManager.DataAccess;
using CRMManager.Models;

namespace CRMManager.Controllers
{
    public class GuestController : Controller
    {
        private IEnumerable<SelectListItem> CourtsMaster()
        {
            List<SelectListItem> objReturnList = new List<SelectListItem>
            {
                new SelectListItem {Text = "C1", Value = "C1"},
                new SelectListItem {Text = "C2", Value = "C2"}
            };
            return objReturnList;
        }

        private IEnumerable<SelectListItem> TimeSlotMaster()
        {
            var membersDao = new MembersDao();
            var timeSlots = membersDao
                .TimeSlotMaster()
                .Select(x =>
                    new SelectListItem
                    {
                        Value = x.SlotId.ToString(),
                        Text = x.SlotName
                    });

            return new SelectList(timeSlots, "Value", "Text");
        }

        private IEnumerable<SelectListItem> PaymentModeMaster()
        {
            List<SelectListItem> objReturnList = new List<SelectListItem>
            {
                new SelectListItem {Text = "CARD", Value = "CARD"},
                new SelectListItem {Text = "CASH", Value = "CASH"}
            };
            return objReturnList;
        }       

        public ActionResult GuestSearch()
        {
            ViewBag.Message = "Please type in 10 digit guest MOBILE number to search";
            return View("GuestSearch");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuestSearchActionSubmit(Guest guest)
        {
            ViewBag.Message = "Please type in 10 digit guest MOBILE number to search";
            if (string.IsNullOrEmpty(guest.GuestMobile))
            {
                return View("GuestSearch");
            }

            var guestDao = new GuestDao();
            List<Guest> guestSearch = guestDao.GuestSearch(guest, "GuestMobile");
            ViewData["SearchResult"] = guestSearch;
            return View("GuestSearch");
        }

        [HttpGet]
        public ActionResult GuestPaymentForm(string guestMobileNumber, string guestName)
        {
            var model = new Guest()
            {
                GuestTimeslotOptions = TimeSlotMaster(),
                GuestCourtOptions = CourtsMaster(),
                PaymentModeOptions = PaymentModeMaster(),
                GuestName = guestName,
                GuestMobile = guestMobileNumber
            };
            return View("GuestPayment", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuestPaymentSubmit(Guest guest)
        {
            if (!ModelState.IsValid)
            {
                return View("GuestPayment");
            }

            GuestDao guestDao = new GuestDao();
            try
            {
                guestDao.GuestPayment(guest);
                ViewBag.Message = "Thank you for your payment " + guest.GuestName;
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View("Error");
            }

            return View("GuestSearch");
        }

        [HttpGet]
        public ActionResult GuestCancel(string memberPaymentId)
        {
            if (memberPaymentId != null && !memberPaymentId.Equals(""))
            {
                new GuestDao().GuestBookingCancel(memberPaymentId);
            }
            ViewBag.Message = "We have cancelled booking ID " + memberPaymentId;
            return Redirect("/Payments/GuestCollectionReport");
        }

    }
}
