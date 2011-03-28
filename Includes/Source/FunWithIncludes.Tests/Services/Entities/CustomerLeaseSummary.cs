using System;

namespace FunWithIncludes.Tests.Services.Entities
{
    public class CustomerLeaseSummary 
    {
        #region Properties

        public string LeaseId { get; set; }

        public string BuildingId { get; set; }
        public string UnitId { get; set; }
        public DateTime? LeaseOccupyDate { get; set; }
        public DateTime? LeaseVacateDate { get; set; }
        public DateTime? ResidentOccupyDate { get; set; }
        public DateTime? ResidentVacateDate { get; set; }
        public string UnitAddress { get; set; }

        public Community Community { get; set; }

        #endregion


        #region IRavenDocument Members

        public string Id { get; set; }


        #endregion
    }
}