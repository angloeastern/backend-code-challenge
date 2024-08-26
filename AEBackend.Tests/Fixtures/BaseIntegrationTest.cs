using System.Text;
using System.Text.Json;
using AEBackend.Repositories.RepositoryUsingEF;
using AEBackend.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public abstract class BaseIntegrationTest
    : IClassFixture<TestApiFixture>, IDisposable
{
  public BaseIntegrationTest(TestApiFixture fixture)
  {
    _serviceScope = fixture.Services.CreateScope();
    _httpClient = fixture.CreateClient();
  }

  protected IServiceScope _serviceScope;
  protected HttpClient _httpClient;
  private JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
  {
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
  };

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
  }
}