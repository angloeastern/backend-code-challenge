using System.Text;
using System.Text.Json;
using AEBackend;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;

namespace AEBackend.Tests.ControllerTests;

public class BaseControllerTest : IClassFixture<CustomWebApplicationFactory>
{

  private readonly CustomWebApplicationFactory _factory;
  protected IServiceScope _serviceScope;
  protected HttpClient _httpClient;
  protected ILogger<BaseControllerTest> _logger;
  public BaseControllerTest(CustomWebApplicationFactory factory)
  {
    _factory = factory;

    _serviceScope = factory.Services.CreateScope();
    _httpClient = factory.CreateClient();
    _logger = _serviceScope.ServiceProvider.GetService<ILogger<BaseControllerTest>>();
  }

  public async Task<string> GetLoginToken()
  {
    var loginRequest = new HttpRequestMessage(HttpMethod.Post, "api/v1/login");
    var loginData = new Dictionary<string, string>{
      {"email", "irwansyah@gmail.com"},
      {"password", "Abcd1234!"},
    };
    var jsonData = JsonSerializer.Serialize(loginData);
    var contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");
    loginRequest.Content = contentData;

    var loginResponse = await _httpClient.SendAsync(loginRequest);
    var loginResponseString = await loginResponse.Content.ReadAsStringAsync();

    _logger.LogDebug("loginResponseString:" + loginResponseString);

    var responseData = JsonSerializer.Deserialize<Dictionary<string, object>>(loginResponseString);

    return responseData["data"].ToString();

  }

}