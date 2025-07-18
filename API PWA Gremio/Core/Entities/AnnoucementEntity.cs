using PWA_GREMIO_API.Core.Entities.Groups;
using PWA_GREMIO_API.Core.Entities.Users;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PWA_GREMIO_API.Core.Entities
{
    public class AnnoucementEntity : Auditable<int?>
    {

        public required string? Text { get; set; }
        public required string? Title { get; set; }
        public required string? Image_url { get; set; }
        public required DateOnly? DateOfExpiration { get; set; }

        public required TimeOnly? TimeOfExpiration { get; set; }

        public required List<int?> DestinationGroupsIds { get; set; } = [] ;

        public  int? AuthorUserSignalRId { get; set; }
        public UserSignalR? AuthorUserSignalR{ get; set; }


    }
}
