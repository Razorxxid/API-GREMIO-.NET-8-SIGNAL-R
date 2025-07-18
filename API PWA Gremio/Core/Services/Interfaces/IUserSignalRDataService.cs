using PWA_GREMIO_API.Core.Dtos;
using PWA_GREMIO_API.Core.Entities.Users;

namespace PWA_GREMIO_API.Core.Services.Interfaces
{
    public interface IUserSignalRDataService
    {

        Task<IEnumerable<AdminGroupListDto>> AdminGroupsOfUser(int? userId);

        Task<IEnumerable<ReceiveAnnoucementDto>> AdminGetUserSentAnnoucements(int? userId);
        Task<IEnumerable<ReceiveAnnoucementDto>> GetAnnoucementsOfUser(int userId);
    }
}
