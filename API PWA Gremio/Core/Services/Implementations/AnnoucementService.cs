using PWA_GREMIO_API.Core.Entities;
using PWA_GREMIO_API.Core.Entities.Groups;
using PWA_GREMIO_API.Core.Entities.Users;
using PWA_GREMIO_API.Core.Repositories;
using PWA_GREMIO_API.Infraestructure;

namespace PWA_GREMIO_API.Core.Services.Implementations
{
    public class AnnoucementService
    {
        private readonly UnitOfWork _unitOfWork;
        public AnnoucementService(UnitOfWork unitofwork) 
        { 
            _unitOfWork = unitofwork;
        }
        
        public async Task<List<AnnoucementEntity?>> GetAnnoucementOfUserAsync(int? userId)
        {
            IRepository<AnnoucementEntity, int?> annoucementRepository =  _unitOfWork.GetRepository<AnnoucementEntity, int?>();
            IRepository<UserSignalR, int?> userSignalRRepository =  _unitOfWork.GetRepository<UserSignalR, int?>();

            int? userSignalRId = await userSignalRRepository.GetProyected(x => x.UserAuthId == userId, x => x.Id);

    

            IEnumerable<AnnoucementEntity?> annoucements =  await annoucementRepository.GetProyectedMany(
                x => x.AuthorUserSignalRId == userSignalRId, x => x);

            return annoucements.ToList();
        }


        
        
    }
}
