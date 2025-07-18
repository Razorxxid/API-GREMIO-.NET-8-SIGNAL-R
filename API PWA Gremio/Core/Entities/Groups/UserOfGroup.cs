using Microsoft.AspNetCore.Identity;
using PWA_GREMIO_API.Core.Entities.Users;

namespace PWA_GREMIO_API.Core.Entities.Groups
{
    public class UserOfGroup : EntityBase<int?>
    {
        public required int? UserSignalRId { get; set; }

        public  UserSignalR UserSignalR { get; set; }

        public required int? GroupId { get; set; }

        public  Group Group { get; set; }

        public required string RoleInGroup { get; set; }

    }
}
