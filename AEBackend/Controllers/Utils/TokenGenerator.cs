using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public static class TokenGenerator
{

  public static string GenerateToken(IConfiguration _configuration, string userName)
  {
    var secret = _configuration["JwtConfig:Secret"];
    var issuer = _configuration["JwtConfig:ValidIssuer"];
    var audience = _configuration["JwtConfig:ValidAudiences"];

    if (secret is null || issuer is null || audience is null)
    {
      throw new ApplicationException("Jwt is not set in the configuration");
    }

    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userName) }),
      Expires = DateTime.UtcNow.AddDays(1),
      Issuer = issuer,
      Audience = audience,
      SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
    };
    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
    var token = tokenHandler.WriteToken(securityToken); return token;
  }

}