
using PWA_GREMIO_API.Core.Entities.Users;
using PWA_GREMIO_API.Core.Repositories;
using PWA_GREMIO_API.Core.Services.Interfaces;

namespace PWA_GREMIO_API.Core.Services.Implementations
{
    public class UserDataService : IUserDataService
    {
        

        private readonly IUnitOfWork _unitOfWork;


        public UserDataService(IUnitOfWork unitOfWork) 
        {
            
            _unitOfWork = unitOfWork;
        }

        
        public async Task<UserData> GetUserData(int id)
        {

            var userDataRepository = _unitOfWork.GetRepository<UserData, int?>();

            try
            {
            
                Console.WriteLine("Getting user data");
                Console.WriteLine("UserAuthId: " + id);

                var userData = await userDataRepository.GetProyected(x => x.UserAuthId == id, x => x) ??
                    throw new Exception("UserData not found");

                Console.WriteLine("userData: " + userData.Name + " Found");


                return userData;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task CreateUserData(UserData userData)
        {
            var userDataRepository = _unitOfWork.GetRepository<UserData, int?>();

            try
            {
                await userDataRepository.Add(userData);
                await _unitOfWork.Commit();


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task UpdateUserData(UserData userData)
        {
            var userDataRepository = _unitOfWork.GetRepository<UserData, int?>();

            try
            {

                userDataRepository.Update(userData);
                await _unitOfWork.Commit();

            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task DeleteUserData(int id)
        {
            var userDataRepository = _unitOfWork.GetRepository<UserData, int?>();

            try
            {
                var userDataEntity = await userDataRepository.GetProyected(x => x.UserAuthId == id, x => x) ??
                    throw new Exception("UserData not found");

                userDataRepository.Delete(userDataEntity);

                await _unitOfWork.Commit();

            } catch (Exception e)
            {

                throw new Exception(e.Message);

            }
        }


    }
}
