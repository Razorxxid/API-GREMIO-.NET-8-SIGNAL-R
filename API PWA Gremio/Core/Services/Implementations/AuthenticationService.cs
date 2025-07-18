
using Microsoft.IdentityModel.Tokens;
using PWA_GREMIO_API.Core.Dtos;
using PWA_GREMIO_API.Core.Entities.Users;
using PWA_GREMIO_API.Core.Exceptions;
using PWA_GREMIO_API.Core.Repositories;
using PWA_GREMIO_API.Core.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PWA_GREMIO_API.Core.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISecurityService _securityService;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IUnitOfWork unitOfWork, ISecurityService securityService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _securityService = securityService;
            _configuration = configuration;
        }

        public async Task<string> GetToken(LoginDto dto)
        {
            var userRepository = _unitOfWork.GetRepository<UserAuth, int?>();
            var hash = await userRepository.GetProyected(q => q.Email == dto.Email, p => p.Password) ?? throw new BusinessNotFoundException("ErrUserNotFound");

            if (!_securityService.Verify(dto.Password, hash)) throw new BusinessException("ErrInvalidEmailOrPassword");

            return GenerateTokenJwt(dto.Email);
        }

        public async Task<int?> GetUser(string email)
        {
            var userRepository = _unitOfWork.GetRepository<UserAuth, int?>();
            int? user = await userRepository.GetProyected(q => q.Email == email, p => p.Id);

                

            return user;
        }
        private string GenerateTokenJwt(string email)
        {
            var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Audience:Secret")));
            var tokenDescription = new SecurityTokenDescriptor
            {
                //Issuer = _configuration.GetValue<string>("Audience:Iss")
                //Audience = _configuration.GetValue<string>("Audience:Aud")
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256Signature),
                Subject = new 
                
                
                (
                    new Claim[]
                    {
                        new(ClaimTypes.NameIdentifier, email),
                    }
               )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }
    }
}
