using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebDev_Labb2_API.Model
{
    public class GetJWT
    {
        public string GenerateJwtToken(string username, string userrole)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, userrole),
                new Claim("username", username),
                new Claim("role", userrole)
            };

            // Lägg till ytterligare claims baserat på rollen
            if (userrole == "Admin")
            {
                claims.Add(new Claim("isAdmin", "true"));
                claims.Add(new Claim("permissions", "full_access"));
            }
            else
            {
                claims.Add(new Claim("isAdmin", "false"));
                claims.Add(new Claim("permissions", "limited_access"));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("I am the administrator, this key is my password, Identify me."));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
