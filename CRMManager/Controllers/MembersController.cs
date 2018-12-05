using System;
using System.Collections.Generic;
using System.Linq;
using CRMManager.DataAccess;
using CRMManager.Models;
using System.Web.Mvc;
using System.Web.SessionState;

namespace CRMManager.Controllers
{
    public class MembersController : Controller
    {
        public ActionResult MemberSearch()
        {
            ViewBag.Message = "Search by 10 digit member Customer Mobile number or Customer Name";
            return View("MemberSearch");
        }

        public ActionResult MemberRegister()
        {
            var model = new Member()
            {
                MemberTimeslotOptions = TimeSlotMaster(),
                MemberCourtOptions = CourtsMaster(),
                MemberTypeOptions = MemberTypeMaster()
            };

            ViewBag.Message = "Please fill in all fields to register a Member";
            return View(model);
        }

        private IEnumerable<SelectListItem> CourtsMaster()
        {
            List<SelectListItem> objReturnList = new List<SelectListItem>
            {
                new SelectListItem {Text = "C1", Value = "C1"},
                new SelectListItem {Text = "C2", Value = "C2"}
            };
            return objReturnList;
        }

        private IEnumerable<SelectListItem> MemberTypeMaster()
        {
            List<SelectListItem> objReturnList = new List<SelectListItem>
            {
                new SelectListItem {Text = "REGULAR", Value = "REGULAR"},
                new SelectListItem {Text = "COACHING", Value = "COACHING"}
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberRegister(Member member)
        {
            if (!ModelState.IsValid)
            {
                var model = new Member()
                {
                    MemberTimeslotOptions = TimeSlotMaster(),
                    MemberCourtOptions = CourtsMaster(),
                    MemberTypeOptions = MemberTypeMaster()
                };
                ViewBag.Message = "Please fill in all fields to register a Member";
                return View("MemberRegister", model);
            }

            MembersDao membersDao = new MembersDao();
            try
            {
                membersDao.MemberRegister(member);
                ViewBag.Message = "Thank you registering " + member.MemberName;
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View("Error");
            }

            return View("MemberSearch");
        }


        [HttpGet]
        public ActionResult MemberUpdateSearch(string profileId)
        {
            if (profileId == null || profileId.Equals(""))
            {
                return View("MemberSearch");
            }

            Member memberUpdate = null;


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

            var model = new Member()
            {
                MemberTimeslotOptions = TimeSlotMaster(),
                MemberTimeslot = memberUpdate.SelectedTimeSlotId,

                MemberCourtOptions = CourtsMaster(),
                MemberCourt = memberUpdate.MemberCourt,

                MemberTypeOptions = MemberTypeMaster(),
                MemberType = memberUpdate.MemberType,

                MemberStatus = memberUpdate.MemberStatus,
            };

            ViewData["SearchResult"] = memberUpdate;
            return View("MemberUpdate", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberViewPayments(Member member)
        {
            if (!ModelState.IsValid)
            {
                return View("MemberUpdate");
            }
            return View("MemberSearch");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberMakePayment(Member member)
        {
            if (!ModelState.IsValid)
            {
                return View("MemberUpdate");
            }
            return View("MemberSearch");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberSearchActionSubmit(Member member)
        {
            ViewBag.Message = "Please type the search key for results";
            if (string.IsNullOrEmpty(member.MemberMobile))
            {
                return View("MemberSearch");
            }

            string selectedKey = member.SearchBy;

            var membersDao = new MembersDao();
            List<Member> memberSearch = membersDao.MemberSearch(member, selectedKey);
            ViewData["SearchResult"] = memberSearch;
            return View("MemberSearch");
        }

        [HttpGet]
        public ActionResult MembersListReport()
        {
            List<Member> membersList = new MembersDao().MembersListReport();
            ViewData["SearchResult"] = membersList;
            return View("MemberListReport");
        }

        [HttpGet]
        public ActionResult MemberSlotAvailability()
        {
            List<Availability> slotAvailabilities = new MembersDao().MemberSlotAvailability();
            ViewData["SearchResult"] = slotAvailabilities;
            ViewBag.Message = "Slot availability for Members";
            return View("MemberSlotAvailability");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberUpdate(Member member)
        {
            if (!ModelState.IsValid)
            {
                return View("MemberUpdate");
            }

            MembersDao membersDao = new MembersDao();
            try
            {
                membersDao.MemberUpdate(member);
                ViewBag.Message = "You have updated " + member.MemberName;
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View("Error");
            }

            return View("MemberSearch");
        }

        [HttpGet]
        public ActionResult ListSlotMembers(string slotId, string courtId)
        {
            if (slotId != null && !slotId.Equals(""))
            {
                List<Member> membersList = new MembersDao().ListSlotMembers(slotId, courtId);
                ViewData["SearchResult"] = membersList;
            }
            return View("ListSlotMembers");
        }

        [HttpGet]
        public ActionResult MemberDeRegister(string memberId)
        {
            if (memberId != null && !memberId.Equals(""))
            {
                string memberName = new MembersDao().MemberDeRegister(memberId);
                ViewBag.Message = "We have De-Registered " + memberId + " - " + memberName;
            }
            return Redirect("MemberSlotAvailability");
        }

        [HttpGet]
        public ActionResult PrintReceipt(string receiptId)
        {
            List<ReceiptPrint> receiptPrint;
            receiptPrint = new MembersDao().PrintPaymentReceipt(receiptId);

            ViewData["ReceiptData"] = receiptPrint[0];
            return View("../PrintReceipt/PrintReceipt");
        }

        [HttpGet]
        public ActionResult MemberLastPaymentReport()
        {
            List<MemberPayment> membersLastPaidList = new PaymentDao().MemberLastPaymentReport();
            ViewData["SearchResult"] = membersLastPaidList;
            //ViewBag.Message = Convert.ToInt32(selectedMonth);
            return View("MemberLastPaymentReport");
        }


    }
}
