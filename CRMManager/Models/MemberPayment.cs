using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRMManager.Models
{
    public class MemberPayment
    {
        
        public string MemberCode { get; set; }
        
        public string MemberMobile { get; set; }
        
        public string MemberName { get; set; }

        public string MemberType { get; set; }

        public string TimeSlot { get; set; }

        public string PaymentDate { get; set; }

        public bool IsActive { get; set; }
        
        public string PaymentAmount { get; set; }
        
    }
}