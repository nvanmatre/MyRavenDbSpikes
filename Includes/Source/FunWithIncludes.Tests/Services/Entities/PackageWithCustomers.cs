using System.Collections.Generic;

namespace FunWithIncludes.Tests.Services.Entities
{
    public class PackageWithCustomers
    {
        public List<Customer> Customers { get; set; }
        public Package Package { get; set; }
    }
}