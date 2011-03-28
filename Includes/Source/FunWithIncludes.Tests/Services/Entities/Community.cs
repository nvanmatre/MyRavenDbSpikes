namespace FunWithIncludes.Tests.Services.Entities
{
    public class Community 
    {

        public string CommunityCode { get; set; }
        public string SubCommunityCode { get; set; }
        public string CommunityName { get; set; }
        public bool IsActive { get; set; }

        #region IRavenDocument Members

        public string Id { get; set; }

        #endregion

    }
}