using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web.Mvc;

namespace CRMManager.Models
{
    public class SysReceipts
    {
        public string ReceiptsCount { get; set; }

        public string PaymentDate { get; set; }

        public string PaymentMode { get; set; }

        public string PaymentAmount { get; set; }

        public string RegFeesAmt { get; set; }
       
        public string SysRefNumber { get; set; }
    }
}