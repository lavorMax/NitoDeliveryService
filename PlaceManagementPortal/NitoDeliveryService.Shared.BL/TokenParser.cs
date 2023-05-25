using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace NitoDeliveryService.Shared.BL
{
    public class TokenParser : ITokenParser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenParser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public UserMetadata GetMetadata()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);
            var userMetadata = decodedToken.Payload["metadata"].ToString();

            var metadata = JsonConvert.DeserializeObject<UserMetadata>(userMetadata);

            return metadata;
        }
    }
}
