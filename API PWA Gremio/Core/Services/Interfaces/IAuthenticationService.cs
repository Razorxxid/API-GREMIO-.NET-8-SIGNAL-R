using PWA_GREMIO_API.Core.Dtos;

namespace PWA_GREMIO_API.Core.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> GetToken(LoginDto dto);
        Task<int?> GetUser(string email);
    }
}
