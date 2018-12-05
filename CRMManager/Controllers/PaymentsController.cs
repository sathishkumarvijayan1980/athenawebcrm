using System;
using System.Collections.Generic;
using CRMManager.DataAccess;
using CRMManager.Models;
using System.Web.Mvc;
using System.Web.SessionState;

namespace CRMManager.Controllers
{
    public class PaymentsController : Controller
    {

        [HttpGet]
        public ActionResult MemberSearchForPayment(string profileId)
        {
            if (profileId == null || profileId.Equals(""))
            {
                return View("Error");
            }

            Member memberUpdate = null;

            var model = new Payment()
            {
                PaymentModeOptions = PaymentModeMaster(),
                PaymentTowardsMaster = PaymentTowardsMaster(),
            };

            try
            {
                memberUpdate = new MembersDao().MemberUpdateSearch(profileId);
                ViewBag.Message = "You are editing " + memberUpdate.MemberName;
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View("Error");
            }

            ViewData["SearchResult"] = memberUpdate;
            return View("Payment", model);
        }

        [HttpGet]
        public ActionResult PaymentsHistorySearch(string profileId)
        {
            if (profileId == null || profileId.Equals(""))
            {
                return View("Error");
            }

            List<Payment> payments = null;

            try
            {
                payments = new PaymentDao().PaymentsSearch(profileId, "MemberCode");
                if (payments != null && payments.Count > 0)
                {
                    ViewBag.Message = "You are editing " + payments[0].MemberName;
                    ViewData["SearchResult"] = payments;
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View("Error");
            }

            return View("PaymentHistory");
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

        private IEnumerable<SelectListItem> PaymentTowardsMaster()
        {
            List<SelectListItem> objReturnList = new List<SelectListItem>
            {
                new SelectListItem {Text = "REG_MEM_FEES", Value = "REG_MEM_FEES"},
                new SelectListItem {Text = "MEM_RENEWAL_FEES", Value = "MEM_RENEWAL_FEES"}
            };
            return objReturnList;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberPayment(Payment payment)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            PaymentDao paymentDao = new PaymentDao();
            try
            {
                paymentDao.InsertMemberPayments(payment);
                ViewBag.Message = "Thank you for payment " + payment.MemberName;
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View("Error");
            }

            return View("PaymentHistory");
        }

        [HttpGet]
        public ActionResult MemberPaymentReport()
        {
            string selectedMonth = this.Request.QueryString["monthId"];
            if(selectedMonth == null || "".Equals(selectedMonth))
            {
                selectedMonth = DateTime.Now.Month.ToString();
            }
            List<PaymentReport> memberPayments = new PaymentDao().ListMemberPayments(Convert.ToInt32(selectedMonth));
            ViewData["SearchResult"] = memberPayments;
            ViewBag.Message = Convert.ToInt32(selectedMonth);
            return View("MemberPaymentReport");
        }

        [HttpGet]
        public ActionResult ReceiptsListReport()
        {
            string selectedMonth = this.Request.QueryString["monthId"];
            if (selectedMonth == null || "".Equals(selectedMonth))
            {
                selectedMonth = DateTime.Now.Month.ToString();
            }
            List<SysReceipts> memberPayments = new PaymentDao().ReceiptsListReport(Convert.ToInt32(selectedMonth));
            ViewData["SearchResult"] = memberPayments;
            ViewBag.Message = Convert.ToInt32(selectedMonth);
            return View("ReceiptsListReport");
        }

        public ActionResult MemberDayWiseCollectionReport()
        {
            List<PaymentReport> memberPayments = null;
            string selectedPaymentDate = this.Request.QueryString["PaymentDate"];
            string displayDate = selectedPaymentDate;
            if (string.IsNullOrEmpty(selectedPaymentDate))
            {
                selectedPaymentDate = DateTime.Now.ToString();
                displayDate = DateTime.Now.ToString("dd/MM/yyyy");
            }
            memberPayments = new PaymentDao().MemberCollectionReport(Convert.ToDateTime(selectedPaymentDate));
            ViewData["SearchResult"] = memberPayments;
            ViewBag.Message = displayDate;
            return View("MemberCollectionsReport");
        }

        [HttpGet]
        public ActionResult GuestPaymentReport()
        {

            string selectedMonth = this.Request.QueryString["monthId"];
            if (selectedMonth == null || "".Equals(selectedMonth))
            {
                selectedMonth = DateTime.Now.Month.ToString();
            }

            List<PaymentReport> guestPayments = new PaymentDao().ListGuestPayments(Convert.ToInt32(selectedMonth));
            ViewData["SearchResult"] = guestPayments;
            ViewBag.Message = Convert.ToInt32(selectedMonth);
            return View("GuestPaymentReport");
        }

        public ActionResult GuestCollectionReport()
        {
            List<PaymentReport> guestPayments = null;
            string selectedPaymentDate = this.Request.QueryString["PaymentDate"];
            string displayDate = selectedPaymentDate;
            if (string.IsNullOrEmpty(selectedPaymentDate))
            {
                selectedPaymentDate = DateTime.Now.ToString();
                displayDate = DateTime.Now.ToString("dd/MM/yyyy");
            }

            guestPayments = new PaymentDao().GuestCollectionReport(Convert.ToDateTime(selectedPaymentDate));
            ViewData["SearchResult"] = guestPayments;
            ViewBag.Message = displayDate;
            return View("GuestCollectionReport");
        }

        [HttpGet]
        public ActionResult PrintReceipt(string profileId)
        {
            return View();
        }
    }
}