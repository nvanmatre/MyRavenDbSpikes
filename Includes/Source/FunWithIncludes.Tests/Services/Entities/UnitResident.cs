namespace FunWithIncludes.Tests.Services.Entities
{
    public class UnitResident
    {
        public string AddressName
        {
            get
            {
                string name = (!string.IsNullOrEmpty(FirstName) ? FirstName : "") +
                              (!string.IsNullOrEmpty(MiddleInitial) ? " " + MiddleInitial : "") +
                              (!string.IsNullOrEmpty(LastName) ? " " + LastName : "");
                return UnitAddress + (name.Length > 0 ? " - " + name : "");
            }
        }

        public string BuildingId { get; set; }
        public string CommunityCode { get; set; }
        public string FirstName { get; set; }
        public string Id { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string SubCommunityCode { get; set; }
        public string UnitAddress { get; set; }
        public string UnitId { get; set; }
    }
}