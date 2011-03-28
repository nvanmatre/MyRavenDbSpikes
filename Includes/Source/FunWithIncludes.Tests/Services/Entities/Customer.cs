using System;
using System.Collections.Generic;

namespace FunWithIncludes.Tests.Services.Entities
{
    public class Customer 
    {

        public Customer()
        {
            LeaseHistory = new List<CustomerLeaseSummary>();
            Emails = new List<CustomerEmail>();
            Phones = new List<CustomerPhone>();
        }
        #region Properties

        public DateTime? DOB { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public List<CustomerLeaseSummary> LeaseHistory { get; set; }

        public List<CustomerEmail> Emails { get; set; }
        public List<CustomerPhone> Phones { get; set; }


        #endregion

        #region IRavenDocument Members

        public string Id { get; set; }

        #endregion
    }
}