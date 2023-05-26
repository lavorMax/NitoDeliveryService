using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories
{
    public class TokenParser : ITokenParser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenParser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            var handler = new JwtSecurityTokenHandler();

            var decodedToken = handler.ReadJwtToken(token.Split(' ')[1]);
            var userMetadata = decodedToken.Subject.ToString();

            return userMetadata;
        }
    }
}
