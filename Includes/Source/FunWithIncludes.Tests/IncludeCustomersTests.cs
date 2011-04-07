using System;
using System.Collections.Generic;
using System.Diagnostics;
using FunWithIncludes.Tests.Services;
using FunWithIncludes.Tests.Services.Entities;
using FunWithIncludes.Tests.Services.Indexes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;

namespace FunWithIncludes.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class IncludeCustomersTests : AbstractBaseTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //


        [TestMethod]
        public void LoadDoesNotRoundtripOnIncludedCustomer()
        {
            var newSession = RavenDBConfiguration.Instance().OpenNewSession();

            var customer1 = new Customer
            {
                FirstName = "Test1",
                LastName = "User",
                Phones =
                    new List<CustomerPhone> { new CustomerPhone { Number = "11111111", PhoneType = PhoneTypes.WorkPhone1 } }
            };
            newSession.Store(customer1);

            var customer2 = new Customer
            {
                FirstName = "Test2",
                LastName = "User",
                Phones =
                    new List<CustomerPhone> { new CustomerPhone { Number = "22222222", PhoneType = PhoneTypes.WorkPhone1 } }
            };
            newSession.Store(customer2);

            var unitResident = new UnitResident
            {
                CommunityCode = "101",
                SubCommunityCode = "101",
                BuildingId = "01",
                UnitId = "0420",
                UnitAddress = "4501 Connecticut Ave NW #420",
                LastName = "",
                FirstName = ""
            };

            var package1 = new Package
            {
                UnitResident = unitResident,
                CommunityCode = "101",
                CustomerIds = new List<string> { customer1.Id, customer2.Id },
                PackageType = "FedEx",
                Status = PackageStatusCodes.Received.ToString()
            };
            newSession.Store(package1);
            newSession.SaveChanges();

            
            var service = new NotificationService();
            var currentSession = AbstractRavenService.GetCurrentDocumentSession();
            
            var packages = service.GetPackageInventory("101", string.Empty, false, false, null, null, 10);

            Assert.AreEqual(1, currentSession.Advanced.NumberOfRequests);

            Assert.IsNotNull(packages);
            foreach (var package in packages)
            {
                Assert.IsTrue(package.Customers.Count > 0);
            }
        }

        [TestInitialize]
        public void Setup()
        {         
            CreateHttpContext();
            RavenDBConfiguration.Instance().Init();
        }



    }
}