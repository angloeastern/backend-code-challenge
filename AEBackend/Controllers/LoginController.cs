using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AEBackend.Controllers;
using AEBackend.DomainModels;
using Asp.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;

namespace AEBackend.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class LoginController : ApplicationController
{

  private UserManager<User> _userManager;
  private IConfiguration _configuration;

  public LoginController(UserManager<User> userManager, IConfiguration configuration)
  {
    _userManager = userManager;
    _configuration = configuration;
  }


  [SwaggerOperation("Logging in a user and retrieve the token to call the API")]
  [EnableRateLimiting("fixed")]
  [HttpPost(Name = "Login")]
  [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ApiResult), StatusCodes.Status400BadRequest)]
  [ProducesResponseType(typeof(ApiResult), StatusCodes.Status500InternalServerError)]
  public async Task<ApiResult<string>> Login([FromBody] LoginRequest loginRequest)
  {
    try
    {
      if (ModelState.IsValid)
      {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null)
        {
          return ApiResult.Failure<string>("Invalid user name or password");
        }

        bool isPasswordValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
        if (!isPasswordValid)
        {
          return ApiResult.Failure<string>("Invalid user name or password");
        }

        var token = GenerateToken(user.UserName);
        return ApiResult.Success<string>(token);
      }
      else
      {
        return ApiResult.Failure<string>(string.Join(", ", GetModelStateErrors()));
      }

    }
    catch (System.Exception ex)
    {
      return ApiResult.Failure<string>(new ApiError(ex.ToString()));
    }
  }

  private string? GenerateToken(string userName)
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