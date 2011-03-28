using System.Linq;
using FunWithIncludes.Tests.Services.Entities;
using Raven.Client.Indexes;
using Raven.Database.Indexing;

namespace FunWithIncludes.Tests.Services.Indexes
{
    public class Customer_UnitResidentSearchIndex : AbstractIndexCreationTask<Customer, UnitResident>
    {
        #region Constructors

        public Customer_UnitResidentSearchIndex()
        {
            Map = customers =>
                  from doc in customers
                  from docLeaseHistoryItem in doc.LeaseHistory.DefaultIfEmpty()
                  select new
                             {
                                 FirstName = (doc.FirstName == null ? "" : doc.FirstName),
                                 MiddleInitial = (doc.MiddleInitial == null ? "" : doc.MiddleInitial),
                                 LastName = (doc.LastName == null ? "" : doc.LastName),
                                 CommunityCode = (docLeaseHistoryItem != null) ? docLeaseHistoryItem.Community.CommunityCode : "",
                                 SubCommunityCode = (docLeaseHistoryItem != null) ? docLeaseHistoryItem.Community.SubCommunityCode : "",
                                 BuildingId = (docLeaseHistoryItem != null) ? docLeaseHistoryItem.BuildingId : "",
                                 UnitId = (docLeaseHistoryItem != null) ? docLeaseHistoryItem.UnitId : "",
                                 UnitAddress = (docLeaseHistoryItem != null) ? docLeaseHistoryItem.UnitAddress : ""
                             };

            Stores.Add(x => x.FirstName, FieldStorage.Yes);
            Stores.Add(x => x.LastName, FieldStorage.Yes);
            Stores.Add(x => x.MiddleInitial, FieldStorage.Yes);
            Stores.Add(x => x.CommunityCode, FieldStorage.Yes);
            Stores.Add(x => x.SubCommunityCode, FieldStorage.Yes);
            Stores.Add(x => x.BuildingId, FieldStorage.Yes);
            Stores.Add(x => x.UnitId, FieldStorage.Yes);
            Stores.Add(x => x.UnitAddress, FieldStorage.Yes);
        }

        #endregion
    }
}