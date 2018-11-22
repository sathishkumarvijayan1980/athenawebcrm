using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web.Mvc;

namespace CRMManager.Models
{
    // This class is used for Member and Guest Payment Report
    public class PaymentReport
    {
        [Required(ErrorMessage = "Please provide Member Payment Id", AllowEmptyStrings = false)]
        [DisplayName("PaymentId")]
        public string MemberPaymentId { get; set; }

        [Required(ErrorMessage = "Please provide Member Code", AllowEmptyStrings = false)]
        public string MemberCode { get; set; }

        [Required(ErrorMessage = "Please provide Name", AllowEmptyStrings = false)]
        [DisplayName("Name")]
        public string MemberName { get; set; }

        [Required(ErrorMessage = "Please Provide Receipt Number", AllowEmptyStrings = false)]
        public string ReceiptNumber { get; set; }

        [Required(ErrorMessage = "Please provide Mobile", AllowEmptyStrings = false)]
        [DisplayName("Mobile")]
        public string MemberMobile { get; set; }

        [Required(ErrorMessage = "Please provide Type", AllowEmptyStrings = false)]
        [DisplayName("Type")]
        public string MemberType { get; set; }
        
        [Required(ErrorMessage = "Please provide Timeslot", AllowEmptyStrings = false)]
        [DisplayName("Timelot")]
        public string MemberTimeslot { get; set; }

        [Required(ErrorMessage = "Please Provide Payment Date", AllowEmptyStrings = false)]
        public string PaymentDate { get; set; }

        [Required(ErrorMessage = "Please Provide Payment Amount", AllowEmptyStrings = false)]
        public string PaymentAmount { get; set; }

        [Required(ErrorMessage = "Please Provide Payment Mode", AllowEmptyStrings = false)]
        public string PaymentMode { get; set; }

        [Required(ErrorMessage = "Please Provide Collected By", AllowEmptyStrings = false)]
        public string CollectedBy { get; set; }

        [Required(ErrorMessage = "Please Provide Notes", AllowEmptyStrings = false)]
        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public static explicit operator PaymentReport(DataTable v)
        {
            throw new NotImplementedException();
        }
    }
}