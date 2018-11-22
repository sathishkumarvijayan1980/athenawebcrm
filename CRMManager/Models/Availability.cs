using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CRMManager.Models
{
    public class Availability
    {
        public string MemberCourt { get; set; }

        public string SlotName { get; set; }

        public string MemberCount { get; set; }

        public string Capacity { get; set; }

        public string SlotId { get; set; }
    }
}