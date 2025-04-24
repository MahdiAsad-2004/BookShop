using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BookShop.WebApi.Services
{
    public class JwtService
    {
        //private readonly IConfiguration _config;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        public JwtService(IConfiguration config)
        {
            _jwtKey = config["Jwt:Key"]!;
            _jwtIssuer = config["Jwt:Issuer"]!;
            ArgumentNullException.ThrowIfNull(_jwtKey, nameof(_jwtKey));
            ArgumentNullException.ThrowIfNull(_jwtIssuer, nameof(_jwtIssuer));
        }

        public string GenerateToken(Guid id,string username, string? role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, username),
            };
            if(string.IsNullOrEmpty(role) == false)
                claims.Add(new Claim(ClaimTypes.Role , role));
            
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            SigningCredentials credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken jwtToken = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtIssuer,
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }

}
