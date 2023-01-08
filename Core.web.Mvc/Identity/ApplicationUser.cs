using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.web.Mvc.Identity
{
    public class ApplicationUser : IdentityUser<string>
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string FirstName { get; set; }

        public string OtherNames { get; set; }

        public DateTime CreatedDate { get; set; }

        public long? CustomerId { get; set; }

        public bool IsEnabled { get; set; }

        public Guid? DesignationId { get; set; }

        public bool IsExternalUser { get; set; }

        public byte RecordStatus { get; set; }

        public string CreatedBy { get; set; }

        public string ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public string EditedBy { get; set; }

        public DateTime? EditDate { get; set; }

        public string Remarks { get; set; }

        public DateTime? LastPasswordChangedDate { get; set; }

        public DateTime? LastLoginDateTime { get; set; }
    }

    public class ApplicationUserProperties
    {
        public const string CustomerId = "CustomerId";

        public const string FullName = "FullName";
    }
}
