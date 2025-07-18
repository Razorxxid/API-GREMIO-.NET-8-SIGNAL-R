using PWA_GREMIO_API.Core.Dtos;
using PWA_GREMIO_API.Core.Entities;

namespace PWA_GREMIO_API.Core.Services.Interfaces
{
    public interface IClientService
    {
        public Task RemoveMessageFromList(string message, int annoucementId);

        public Task RemoveMessageFromList(int id);


        public Task SendAnnoucementToGroups(SendAnnoucementDto dto);


        public Task SendAsync(string header,ReceiveAnnoucementDto annoucement);

        public Task SendAsync(string header, int id);


        public Task ReceiveAnnoucement(ReceiveAnnoucementDto annoucement);

    }
}
