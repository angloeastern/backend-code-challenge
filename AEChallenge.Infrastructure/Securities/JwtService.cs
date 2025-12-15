using AEChallenge.Application.Interfaces;
using AEChallenge.Application.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AEChallenge.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly byte[] _key;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _tokenLifetimeDays;
        private readonly int _defaultTokenLifetimeDays = 7;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = _configuration["JwtSettings:SecretKey"]
                     ?? throw new ArgumentNullException("SecretKey not found.");
            _key = Encoding.ASCII.GetBytes(_secretKey);

            _issuer = _configuration["JwtSettings:Issuer"] ?? string.Empty;
            _audience = _configuration["JwtSettings:Audience"] ?? string.Empty;

            if (!int.TryParse(_configuration["JwtSettings:TokenLifetimeDays"], out _tokenLifetimeDays))
            {
                _tokenLifetimeDays = _defaultTokenLifetimeDays;
            }
        }

        public string GenerateToken(UserLoginReadModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.UserRole)
                ]),


                Expires = DateTime.UtcNow.AddDays(_tokenLifetimeDays),
                Issuer = _issuer,
                Audience = _audience,

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(_key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}