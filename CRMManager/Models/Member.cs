using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CRMManager.Models
{
    public class Member
    {
        [Required(ErrorMessage = "Please provide Member Code", AllowEmptyStrings = false)]
        public string MemberCode { get; set; }

        [Required(ErrorMessage = "Please provide Name", AllowEmptyStrings = false)]
        public string MemberName { get; set; }

        [Required(ErrorMessage = "Please provide Address", AllowEmptyStrings = false)]
        public string MemberAddress { get; set; }

        [Required(ErrorMessage = "Please provide Mobile", AllowEmptyStrings = false)]
        public string MemberMobile { get; set; }

        [Required(ErrorMessage = "Please provide DOB", AllowEmptyStrings = false)]
        public string MemberDob { get; set; }

        [Required(ErrorMessage = "Please provide Type", AllowEmptyStrings = false)]
        public string MemberType { get; set; }
        public IEnumerable<SelectListItem> MemberTypeOptions { get; set; }

        [Required(ErrorMessage = "Please provide Timeslot", AllowEmptyStrings = false)]
        public string MemberTimeslot { get; set; }
        public IEnumerable<SelectListItem> MemberTimeslotOptions { get; set; }

        [Required(ErrorMessage = "Please provide Court Allocated", AllowEmptyStrings = false)]
        public string MemberCourt { get; set; }
        public IEnumerable<SelectListItem> MemberCourtOptions { get; set; }
        
        public bool MemberStatus { get; set; }
        
        // This is used during edit operation to set the Member Selected SlotID
        public string SelectedTimeSlotId { get; set; }

        public string SearchBy { get; set; }
    }
}