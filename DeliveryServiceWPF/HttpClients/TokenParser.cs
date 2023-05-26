using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace DeliveryServiceWPF.HttpClients
{
    public static class TokenParser
    {
        public static string GetUserId(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(token);

            var userMetadata = decodedToken.Payload["metadata"].ToString();

            var metadata = JsonConvert.DeserializeObject<string>(userMetadata);

            return metadata;
        }
    }
}
