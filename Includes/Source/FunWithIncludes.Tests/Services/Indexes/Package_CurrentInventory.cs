using System.Linq;
using FunWithIncludes.Tests.Services.Entities;
using Raven.Client.Indexes;

namespace FunWithIncludes.Tests.Services.Indexes
{
    public class Package_CurrentInventory :
        AbstractIndexCreationTask<Package>
    {
        public Package_CurrentInventory()
        {
            Map = packages =>
                  from doc in packages
                  select new
                             {
                                 doc.CommunityCode,
                                 doc.Status,
                                 doc.IsDeleted,
                                 doc.UnitResident.AddressName,
                                 doc.ReceiveDate,
                                 doc.PackageType,
                                 doc.TrackingNumber
                             };
        }
    }
}