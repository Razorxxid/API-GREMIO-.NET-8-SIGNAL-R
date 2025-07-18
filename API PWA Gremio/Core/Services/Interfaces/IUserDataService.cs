using PWA_GREMIO_API.Core.Dtos;
using PWA_GREMIO_API.Core.Entities.Users;

namespace PWA_GREMIO_API.Core.Services.Interfaces
{
    public interface IUserDataService
    {


        Task DeleteUserData(int id);
        Task UpdateUserData(UserData userData);
        Task CreateUserData(UserData userData);
        Task<UserData> GetUserData(int id);
    }
}
