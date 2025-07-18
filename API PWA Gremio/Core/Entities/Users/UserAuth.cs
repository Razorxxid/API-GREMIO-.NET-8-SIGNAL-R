using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.AccessControl;

namespace PWA_GREMIO_API.Core.Entities.Users
{

    public class UserAuth : Auditable<int?>
    { 
        public required int? AffilliateNumber { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }

        public required List<string> Roles { get; set; }

    }

}
