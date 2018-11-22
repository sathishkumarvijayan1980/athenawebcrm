using System;
using CRMManager.Models;

namespace CRMManager.Models
{
    public class ReceiptPrint
    {
        public string BrandName { get; set; } = "Athena Badminton Academy";
        public string BrandAddress1 { get; set; } = "No: 38, Kundrakudi Nagar Mainroad, Adambakkam, CH - 88";
        public string BrandAddress2 { get; set; } = "(P) +91 6381822087 / (044) 48631488";

        public string MemberCode { get; set; }
        public string MemberMobile { get; set; }
        public string MemberName { get; set; }
        public string MemberType { get; set; }

        public string PaymentDate { get; set; }
        public string PaymentPeriod { get; set; }
        public string PaymentAmount { get; set; }
        public string PaymentDesc
        {
            get
            {
                return CommonUtility.ConvertToWords(PaymentAmount+"");
            }

            set
            {

            }
        }

        public string PaymentMode { get; set; } = "Cash / Card";
        public string ReceiptNumber { get; set; }
        public string SysRefNumber { get; set; }
        public string Timeslot { get; set; }

        public string CollectedBy { get; set; }
        public string PrintDt { get; set; } = DateTime.Now.ToString();

    }
}