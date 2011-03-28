using System.Linq;
using FunWithIncludes.Tests.Services.Entities;
using Raven.Client.Indexes;

namespace FunWithIncludes.Tests.Services.Indexes
{
    public class Customer_SearchIndex : AbstractIndexCreationTask<Customer>
    {
        #region Constructors

        public Customer_SearchIndex()
        {
            Map = customers =>
                  from doc in customers
                  from docPhonesItem in doc.Phones.DefaultIfEmpty()
                  from docLeaseHistoryItem in doc.LeaseHistory.DefaultIfEmpty()
                  from docEmailsItem in doc.Emails.DefaultIfEmpty()
                  select new
                             {
                                 doc.FirstName,
                                 doc.LastName,
                                 Number = (docPhonesItem != null) ? docPhonesItem.Number : "",
                                 LeaseId = (docLeaseHistoryItem != null) ? docLeaseHistoryItem.LeaseId : "",
                                 BuildingId = (docLeaseHistoryItem != null) ? docLeaseHistoryItem.BuildingId : "",
                                 UnitId = (docLeaseHistoryItem != null) ? docLeaseHistoryItem.UnitId : "",
                                 Address = (docEmailsItem != null) ? docEmailsItem.Address : "",
                                 CommunityCode = (docLeaseHistoryItem != null) ? docLeaseHistoryItem.Community.CommunityCode : "",
                                 CommunityName = (docLeaseHistoryItem != null) ? docLeaseHistoryItem.Community.CommunityName : ""
                             };
        }

        #endregion
    }
}