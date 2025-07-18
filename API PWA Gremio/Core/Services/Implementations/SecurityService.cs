using PWA_GREMIO_API.Core.Services.Interfaces;

namespace PWA_GREMIO_API.Core.Services.Implementations
{
    public class SecurityService : ISecurityService
    {
        public bool Verify(string password, string hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch { return false; }
        }
    }
}
