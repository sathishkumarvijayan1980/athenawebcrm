using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CRMManager.Models
{
    public class Guest
    {
        [Required(ErrorMessage = "Please provide Guest Name", AllowEmptyStrings = false)]
        public string GuestName { get; set; }

        [Required(ErrorMessage = "Please provide Guest Mobile", AllowEmptyStrings = false)]
        public string GuestMobile { get; set; }

        [Required(ErrorMessage = "Please provide Payment Date", AllowEmptyStrings = false)]
        public string PaymentDate { get; set; }

        [Required(ErrorMessage = "Please provide Payment Amount", AllowEmptyStrings = false)]
        public string PaymentAmount { get; set; }

        [Required(ErrorMessage = "Please provide Receipt Number", AllowEmptyStrings = false)]
        public string ReceiptNumber { get; set; }

        [Required(ErrorMessage = "Please Provide Payment Mode", AllowEmptyStrings = false)]
        public string PaymentMode { get; set; }
        public IEnumerable<SelectListItem> PaymentModeOptions { get; set; }

        [Required(ErrorMessage = "Please provide Collected By", AllowEmptyStrings = false)]
        public string CollectedBy { get; set; }

        [Required(ErrorMessage = "Please provide Notes", AllowEmptyStrings = false)]
        public string Notes { get; set; }

        [Required(ErrorMessage = "Please provide Timeslot", AllowEmptyStrings = false)]
        public string GuestTimeslot { get; set; }
        public IEnumerable<SelectListItem> GuestTimeslotOptions { get; set; }

        [Required(ErrorMessage = "Please provide Court Allocated", AllowEmptyStrings = false)]
        public string GuestCourt { get; set; }
        public IEnumerable<SelectListItem> GuestCourtOptions { get; set; }
    }
}