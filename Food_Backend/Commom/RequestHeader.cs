using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Food_Backend.Commom
{
    public class RequestHeader : IRequestHeader
    {
        public IHttpContextAccessor _httpContextAccessor { get; }
        public RequestHeader(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string UserId => _httpContextAccessor?.HttpContext?.User?.FindFirst("UserId")?.Value;    }
    public interface IRequestHeader
    {
        string UserId { get; }
    }
}
