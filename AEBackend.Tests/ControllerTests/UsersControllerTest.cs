

using System.Net.Http.Json;
using System.Text.Json;
using AEBackend.Controllers;
using AEBackend.DTOs;
using AEBackend.Tests.Fixtures;
using Xunit.Abstractions;

namespace AEBackend.Tests.ControllerTests;

public class UsersControllerTest : BaseIntegrationTest
{
  public UsersControllerTest(TestApiFixture fixture) : base(fixture)
  {
  }

  [Fact]
  public async Task TestApiV1UsersWithGetReturnAllUsers()
  {
    var request = new HttpRequestMessage(HttpMethod.Get, "api/v1/users");

    var response = await _httpClient.SendAsync(request);

    response.EnsureSuccessStatusCode();

    List<User>? responseList = await response.Content.ReadFromJsonAsync<List<User>>();

    Assert.NotNull(responseList);
    Assert.Single(responseList);
    Assert.Equal("irwansyah@gmail.com", responseList[0].Email);

  }
}