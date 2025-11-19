using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using rise_gs.Models;

namespace rise_gs.Services
{
    public class TokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
        }

        public string GenerateToken(Usuario usuario)
        {
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.NomeUsuario ?? string.Empty),
        new Claim(JwtRegisteredClaimNames.Email, usuario.EmailUsuario ?? string.Empty),
        new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
        new Claim(ClaimTypes.Name, usuario.NomeUsuario ?? string.Empty),
        new Claim(ClaimTypes.Role, usuario.TipoUsuario ?? "Aluno")
    };

            // 🔹 garante que NUNCA usamos key vazia
            var keyString = string.IsNullOrWhiteSpace(_jwtSettings.Key)
                ? "SenhaSegura!"   // fallback só pra DEV
                : _jwtSettings.Key;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(
                _jwtSettings.ExpireMinutes > 0 ? _jwtSettings.ExpireMinutes : 60);

            var token = new JwtSecurityToken(
                issuer: string.IsNullOrWhiteSpace(_jwtSettings.Issuer) ? "rise-api" : _jwtSettings.Issuer,
                audience: string.IsNullOrWhiteSpace(_jwtSettings.Audience) ? "rise-clients" : _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
