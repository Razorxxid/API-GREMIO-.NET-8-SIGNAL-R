using System.Security.Claims;

namespace PWA_GREMIO_API.Core.ContextProvider
{
    public class JwtContextProvider : IContextProvider
    {
        private readonly IHttpContextAccessor _context;

        public JwtContextProvider(IHttpContextAccessor context)
        {
            _context = context;
        }
        public string Email => _context?.HttpContext?.User?.Claims.Where(q => q.Type == ClaimTypes.NameIdentifier).Select(q => q.Value).SingleOrDefault();
    }
}
