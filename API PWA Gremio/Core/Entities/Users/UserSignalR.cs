using PWA_GREMIO_API.Core.Entities.Groups;

namespace PWA_GREMIO_API.Core.Entities.Users
{
    public class UserSignalR : Auditable<int?>
    {
        public int? UserAuthId { get; set; }
        public List<string>? ConectionsIdOfUser { get; set; }
        public List<string>? SentAnnoucementsById { get; set; }
            
        public List<UserOfGroup>? GroupsOfUser { get; set; } = [];
        public List<AnnoucementOfGroup> AnnoucementsOfUser { get; set; } = [];

      
    }
}
