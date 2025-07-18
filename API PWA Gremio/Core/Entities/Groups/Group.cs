using PWA_GREMIO_API.Core.Entities.Users;

namespace PWA_GREMIO_API.Core.Entities.Groups
{
    public class Group : Auditable<int?>
    {
        public List<UserOfGroup> UsersOfGroup { get; set; } = [];

        public List<AnnoucementOfGroup> AnnoucementsOfGroup { get; set; } = [];
        public required string Name { get; set; }
        public required string Description { get; set; }

    }
}
