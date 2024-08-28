using System.Text;
using System.Text.Json;
using AEBackend.Repositories.RepositoryUsingEF;
using AEBackend.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Respawn;


public abstract class BaseIntegrationTest
    : IClassFixture<TestApiFixture>, IDisposable
{
  public BaseIntegrationTest(TestApiFixture fixture)
  {
    _fixture = fixture;
    _serviceScope = fixture.Services.CreateScope();
    _httpClient = fixture.CreateClient();
    _logger = _serviceScope.ServiceProvider.GetService<ILogger<BaseIntegrationTest>>();
  }


  protected async Task ResetDB()
  {
    using (var conn = new NpgsqlConnection(_fixture.DBConnectionString))
    {

      await conn.OpenAsync();

      var respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
      {
        SchemasToInclude = new[]
          {
              "public"
          },
        DbAdapter = DbAdapter.Postgres
      });

      await respawner.ResetAsync(conn);
    }
  }
  protected TestApiFixture _fixture;
  protected IServiceScope _serviceScope;
  protected HttpClient _httpClient;
  protected ILogger<BaseIntegrationTest> _logger;
  private JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
  {
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
  };


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

  public async Task<TData?> GetAsync<TData>(string url)
  {
    var result = await _httpClient.GetAsync(url);
    var content = await result.Content.ReadAsStringAsync();
    var data = JsonSerializer.Deserialize<TData>(content, _serializerOptions);

    return data;
  }

  public async Task<TResult?> PostAsync<TData, TResult>(string url, TData data)
  {
    var stringContent = new StringContent(JsonSerializer.Serialize(data),
        Encoding.UTF8, "application/json");

    var result = await _httpClient.PostAsync(url, stringContent);
    var content = await result.Content.ReadAsStringAsync();
    var returnData = JsonSerializer.Deserialize<TResult>(content, _serializerOptions);

    return returnData;
  }

  public void Dispose()
  {
    _serviceScope.Dispose();
    _httpClient.Dispose();
    // _fixture.Dispose();
  }
}