using BookShop.Domain.Identity;
using Microsoft.AspNetCore.Http;

namespace BookShop.Infrastructure.Identity
{
    internal class CurrentWebUser : ICurrentUser
    {

        //private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentWebUser(IHttpContextAccessor httpContextAccessor)
        {
            //_httpContextAccessor = httpContextAccessor;
            Authenticated = httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true ? true : false;
            if (Authenticated)
            {
                bool canGetId = Guid.TryParse(httpContextAccessor.HttpContext?.User.FindFirst(a => a.Type == ClaimTypes.Identifier)?.Value, out Guid id);
                Id = canGetId ? id : null;
                Email = httpContextAccessor.HttpContext?.User.FindFirst(a => a.Type == ClaimTypes.Email)?.Value;
                Name = httpContextAccessor.HttpContext?.User.FindFirst(a => a.Type == ClaimTypes.Name)?.Value;
                Username = httpContextAccessor.HttpContext?.User.FindFirst(a => a.Type == ClaimTypes.Username)?.Value;
                PhoneNumber = httpContextAccessor.HttpContext?.User.FindFirst(a => a.Type == ClaimTypes.PhoneNumber)?.Value;
            }
        }


        public Guid? Id { get; init; }
        public bool Authenticated { get; init; }
        public string? Email { get; init; }
        public string? Name { get; init; }
        public string? Username { get; init; }
        public string? PhoneNumber { get; init; }



    }
}
