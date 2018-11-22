using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRMManager.Models
{
    public class Payment
    {
        [Required(ErrorMessage = "Please Provide Member Code", AllowEmptyStrings = false)]
        public string MemberCode { get; set; }

        [Required(ErrorMessage = "Please Provide Member Phone", AllowEmptyStrings = false)]
        public string MemberMobile { get; set; }

        [Required(ErrorMessage = "Please Provide Member Name", AllowEmptyStrings = false)]
        public string MemberName { get; set; }

        [Required(ErrorMessage = "Please Provide Payment Date", AllowEmptyStrings = false)]
        public string PaymentDate { get; set; }

        [Required(ErrorMessage = "Please Provide Payment Amount", AllowEmptyStrings = false)]
        public string PaymentAmount { get; set; }

        [Required(ErrorMessage = "Please Provide Payment Mode", AllowEmptyStrings = false)]
        public string PaymentMode { get; set; }
        public IEnumerable<SelectListItem> PaymentModeOptions { get; set; }

        [Required(ErrorMessage = "Please Provide Payment Towards", AllowEmptyStrings = false)]
        public string PaymentTowards { get; set; }
        public IEnumerable<SelectListItem> PaymentTowardsMaster { get; set; }

        [Required(ErrorMessage = "Please Provide Collected By", AllowEmptyStrings = false)]
        public string CollectedBy { get; set; }

        [Required(ErrorMessage = "Please Provide Notes", AllowEmptyStrings = false)]
        public string Notes { get; set; }

        public string RegFeesAmount { get; set; }

        public string ReceiptNumber { get; set; }
    }
}