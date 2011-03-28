﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FunWithIncludes.Tests.Services.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;

namespace FunWithIncludes.Tests.Services
{
    public class NotificationService : AbstractRavenService
    {
        public List<PackageWithCustomers> GetPackageInventory(
            string communityCode,
            string sortBy,
            bool sortDescending,
            bool viewIsDeleted,
            DefconFilter defconFilter,
            string unitResidentFilter,
            int? take)
        {
            List<Package> packages = GetPackageInventory(communityCode, sortBy, sortDescending, viewIsDeleted,
                                                         defconFilter, unitResidentFilter, take, true);

            var results = new List<PackageWithCustomers>();
            IDocumentSession currentSession = GetCurrentDocumentSession();

            foreach (Package package in packages)
            {
                var packageWithCustomers = new PackageWithCustomers
                                               {Package = package, Customers = new List<Customer>()};

                //Note:have to loop each customerId. If we load them as a list then that causes a hit to the server :(
                foreach (string customerId in package.CustomerIds)
                {
                    packageWithCustomers.Customers.Add(currentSession.Load<Customer>(customerId));
                }

                results.Add(packageWithCustomers);
            }
            return results;
        }


        public List<Package> GetPackageInventory(
            string communityCode,
            string sortBy,
            bool sortDescending,
            bool viewIsDeleted,
            DefconFilter defconFilter,
            string unitResidentFilter,
            int? take,
            bool inculdeCustomers)
        {
            if (!take.HasValue)
                take = 20;

            IDocumentSession session = GetCurrentDocumentSession();

            IDocumentQuery<Package> query = session.Advanced.LuceneQuery<Package>("Package/CurrentInventory").Take(take.Value);

            if (inculdeCustomers)
            {
                query.Include("CustomerIds");              
            }

            query.WhereEquals("CommunityCode", communityCode);
            query.AndAlso().Not.WhereEquals("Status", PackageStatusCodes.Released);

            if (!viewIsDeleted)
            {
                query.AndAlso();
                query.WhereEquals("IsDeleted", false);
            }

            DefConFilterQuery(query, defconFilter);

            if (!string.IsNullOrEmpty(unitResidentFilter))
            {
                query.AndAlso();
                query.WhereEquals("AddressName", unitResidentFilter);
            }

            if (!string.IsNullOrEmpty((sortBy)))
            {
                query.AddOrder(sortBy, sortDescending);
            }

            var results = query.ToList();

            return results;
        }

        private static void DefConFilterQuery<T>(IDocumentQuery<T> query, DefconFilter defconFilter)
        {
            if (defconFilter == null || (defconFilter.Start == null && defconFilter.End == null)) return;

            DateTime? startDate = null;
            DateTime? endDate = null;

            if (defconFilter.Start.HasValue)
                startDate = DateTime.Today.Date.AddDays(-(defconFilter.Start.Value));

            if (defconFilter.End.HasValue)
                endDate = DateTime.Today.Date.AddDays(-(defconFilter.End.Value));

            query.AndAlso();
            if (startDate.HasValue && endDate.HasValue)
                query.WhereBetweenOrEqual("ReceiveDate", startDate.Value, endDate.Value.AddDays(1));
            else if (!startDate.HasValue)
                query.WhereLessThanOrEqual("ReceiveDate", endDate.Value.AddDays(1));
        }
    }


    public class DefconFilter
    {
        public int? End { get; set; }
        public int? Start { get; set; }
    }
}