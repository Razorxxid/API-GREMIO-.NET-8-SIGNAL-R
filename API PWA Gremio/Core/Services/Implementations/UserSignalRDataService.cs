using PWA_GREMIO_API.Core.Dtos;
using PWA_GREMIO_API.Core.Entities;
using PWA_GREMIO_API.Core.Entities.Groups;
using PWA_GREMIO_API.Core.Entities.Users;
using PWA_GREMIO_API.Core.Repositories;
using PWA_GREMIO_API.Core.Services.Interfaces;
using Group = PWA_GREMIO_API.Core.Entities.Groups.Group;

namespace PWA_GREMIO_API.Core.Services.Implementations
{
    public class UserSignalRDataService : IUserSignalRDataService
    {
        

        private readonly IUnitOfWork _unitOfWork;


        public UserSignalRDataService(IUnitOfWork unitOfWork) 
        {
            
            _unitOfWork = unitOfWork;
        }

        // Obtiene los grupos  a los que pertenece un usuario
        public async Task<IEnumerable<AdminGroupListDto>> AdminGroupsOfUser(int? userId)
{
        IRepository<UserSignalR, int?> userSignalRRepository = _unitOfWork.GetRepository<UserSignalR, int?>();
        IRepository<UserOfGroup, int?> userOfGroupRepository = _unitOfWork.GetRepository<UserOfGroup, int?>();
        IRepository<Group, int?> groupRepository = _unitOfWork.GetRepository<Group, int?>();

        var groups = new List<AdminGroupListDto>();

        try
        {
            // Obtener el ID de UserSignalR
            int? userSignalRId = await userSignalRRepository.GetProyected(x => x.UserAuthId == userId, x => x.Id)
                ;

            // Obtener los IDs de los grupos
            IEnumerable<int?> groupIds = (IEnumerable<int?>)await userOfGroupRepository.GetProyectedMany(
                x => x.UserSignalRId == userSignalRId && x.RoleInGroup.Contains("Admin"), x => x.GroupId);

     

            // Obtener los nombres de los grupos
            IEnumerable<string?> groupNames = await groupRepository.GetProyectedMany(
                x => groupIds.Contains(x.Id), x => x.Name);


                    // Convertir a listas para fácil acceso por índice
             var groupIdsList = groupIds.ToList();

            for (int i = 0; i < groupIdsList.Count(); i++)
            {

                Console.WriteLine("groupId found in db: " + (groupIdsList[i] ));
            }
        
            var groupNamesList = groupNames.ToList();


            for (int i = 0; i < groupNamesList.Count(); i++)
            {

                Console.WriteLine("groupName found in db: " + (groupNamesList[i] ?? "Unknown"));
            }

 
            // Verificar que ambas listas tienen el mismo tamaño
            if (groupIdsList.Count != groupNamesList.Count)
            {
                throw new Exception("Mismatch between number of group IDs and group names.");
            }

            // Crear una lista de DTOs para los grupos
            for (int i = 0; i < groupIdsList.Count; i++)
            {
                groups.Add(new AdminGroupListDto
                {
                    Id = groupIdsList[i],
                    Name = groupNamesList[i] ?? "Unknown"
                });

                Console.WriteLine("Group found in db: " + (groupNamesList[i] ?? "Unknown"));
            }

            return groups;
        }
        catch (Exception e)
        {
            // Log del error antes de lanzarlo
            Console.WriteLine("Exception: " + e.Message);
            throw new Exception(e.Message);
        }
        }


        // Obtiene los anuncios enviados por un usuario específico
        public async Task<IEnumerable<ReceiveAnnoucementDto>> AdminGetUserSentAnnoucements(int? userId)
        {
            IRepository<UserSignalR, int?> userSignalRRepository = _unitOfWork.GetRepository<UserSignalR, int?>();
            IRepository<UserOfGroup, int?> userOfGroupRepository = _unitOfWork.GetRepository<UserOfGroup, int?>();
            IRepository<Group, int?> groupRepository = _unitOfWork.GetRepository<Group, int?>();
            IRepository<AnnoucementEntity, int?> annoucementRepository = _unitOfWork.GetRepository<AnnoucementEntity, int?>();

            var annoucements = new List<ReceiveAnnoucementDto>(); // Cambiado a List en lugar de IEnumerable
           // Console.WriteLine("Executing AdminGetUserSentAnnoucements ");

            try
            {
                int? userSignalRId = await userSignalRRepository.GetProyected(x => x.UserAuthId == userId, x => x.Id);
                //Console.WriteLine("userSignalRId found in db: " + userSignalRId);

                var sentAnnoucementsEntities = await annoucementRepository.GetProyectedMany(
                                       x => x.AuthorUserSignalRId == userSignalRId, x => x);

                foreach (var annoucement in sentAnnoucementsEntities)
                {
                    annoucements.Add(new ReceiveAnnoucementDto
                    {
                        Id = annoucement.Id,
                        Title = annoucement.Title,
                        Text = annoucement.Text,
                        ImageUrl = annoucement.Image_url
                    });
                    Console.WriteLine("Annoucement found in db: " + annoucement.Title);
                }

                foreach (var ann in annoucements)
                {
                    Console.WriteLine("Annoucement found in list: " + ann.Title);
                }

                return annoucements;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Obtiene los datos del usuario por su UserAuthId
        public async Task<UserData> GetUserData(int? userAuthId)
        {

            var userDataRepository = _unitOfWork.GetRepository<UserData, int?>();

            try
            {
            

                var userData = await userDataRepository.GetProyected(x => x.UserAuthId == userAuthId, x => x) ??
                    throw new Exception("UserData not found");

                return userData;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Obtiene los anuncios de un usuario específico
        public async Task<IEnumerable<ReceiveAnnoucementDto>> GetAnnoucementsOfUser(int userId)

        {
      
            var _userSignalRRepository = _unitOfWork.GetRepository<UserSignalR, int?>();


            var userSR = await _userSignalRRepository.GetProyected(f => f.UserAuthId == userId, f => f.Id);

         
            var usersOfGroupsRepository = _unitOfWork.GetRepository<UserOfGroup, int?>();

            IEnumerable<int?> groupsOfUser = await usersOfGroupsRepository.GetProyectedMany(f => f.UserSignalRId == userSR, f => f.Group.Id);


            foreach (var group in groupsOfUser)
            {
                Console.WriteLine("Group " + group);
            }

            var annoucementOfGroupRepository = _unitOfWork.GetRepository<AnnoucementOfGroup, int?>();
            
            IEnumerable<int?> annoucementsOfUser = await annoucementOfGroupRepository.GetProyectedMany(f =>  groupsOfUser.Contains(f.GroupId), f => f.AnnoucementId);


            var annoucementRepository = _unitOfWork.GetRepository<AnnoucementEntity, int?>();

            IEnumerable<AnnoucementEntity?> annoucements = await annoucementRepository.GetProyectedMany(f => annoucementsOfUser.Contains(f.Id), f => f);


            var annoucementDtos = new List<ReceiveAnnoucementDto>();


            if (annoucements is null)
            {
                Console.WriteLine("User has no annoucements");
                return annoucementDtos;
            }

            foreach (var annoucement in annoucements)
            {
                if (annoucement is not null)
                {
                    annoucementDtos.Add(new ReceiveAnnoucementDto
                    {
                        Id = annoucement.Id,
                        Title = annoucement.Title,
                        Text = annoucement.Text,
                        ImageUrl = annoucement.Image_url
                    });
                    Console.WriteLine("Annoucement sent to user " + annoucement.Title);
                }
            }

            return annoucementDtos;
        }
        // Crea un nuevo UserData
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

        // Actualiza los datos del usuario
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

        // Elimina los datos del usuario por su UserAuthId
        public async Task DeleteUserData(int userAuthId)
        {
            var userDataRepository = _unitOfWork.GetRepository<UserData, int?>();

            try
            {
                var userDataEntity = await userDataRepository.GetProyected(x => x.UserAuthId == userAuthId, x => x);

                if (userDataEntity == null)
                {
                    throw new Exception("UserData not found");
                }

                userDataRepository.Delete(userDataEntity);

                await _unitOfWork.Commit();

            } catch (Exception e)
            {

                throw new Exception(e.Message);

            }
        }


    }
}
