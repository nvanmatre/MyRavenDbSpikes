using System.Linq;
using FunWithIncludes.Tests.Services.Entities;
using Raven.Client.Indexes;
using Raven.Database.Indexing;

namespace FunWithIncludes.Tests.Services.Indexes
{
    public class Package_InventoryUnitResidentSearchIndex :
        AbstractIndexCreationTask<Package, Package_InventoryUnitResidentSearchIndex.StringProperty>
    {
        public Package_InventoryUnitResidentSearchIndex()
        {
            Map = packages =>
                  from doc in packages
                  select new
                             {
                                 doc.CommunityCode,
                                 AddressName = (doc.UnitResident.AddressName == null ? "" : doc.UnitResident.AddressName),
                                 doc.IsDeleted,
                                 doc.Status,
                                 doc.ReceiveDate
                             };

            Stores.Add(x => x.AddressName, FieldStorage.Yes);
        }

        public class StringProperty
        {
            public string AddressName { get; set; }
        }
    }
}