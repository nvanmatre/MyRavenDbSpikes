using System;
using System.Collections.Generic;

namespace FunWithIncludes.Tests.Services.Entities
{
    public class Package
    {
        public Package()
        {
            Notifications = new List<Notification>();
        }

        public string CommunityCode { get; set; }
        public List<string> CustomerIds { get; set; }
        public string DeleteNotes { get; set; }
        public string Id { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPerishable { get; set; }
        public DateTime ModifedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Notes { get; set; }
        public List<Notification> Notifications { get; set; }
        public string PackageType { get; set; }
        public DateTime ReceiveDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ReleaseNotes { get; set; }
        public string ReleaseSignature { get; set; }
        public byte[] ReleaseSignatureImage { get; set; }
        public string ReleaseSignaturePath { get; set; }
        public string Status { get; set; }
        public string TrackingNumber { get; set; }
        public string UnitId { get; set; }
        public UnitResident UnitResident { get; set; }

        public string GetDocumentId()
        {
            if (string.IsNullOrEmpty(Id))
                return string.Empty;

            return Id.Replace("packages/", "packages-");
        }

        public bool IsUnitLevel()
        {
            return string.IsNullOrEmpty(UnitResident.Id);
        }
    }
}