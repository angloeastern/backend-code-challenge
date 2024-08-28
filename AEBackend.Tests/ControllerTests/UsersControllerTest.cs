

using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AEBackend.Controllers;
using AEBackend.Controllers.Utils;
using AEBackend.DomainModels;
using AEBackend.DTOs;
using AEBackend.Tests.Fixtures;
using Docker.DotNet.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace AEBackend.Tests.ControllerTests;

public class UsersControllerTest : BaseIntegrationTest
{
  public UsersControllerTest(TestApiFixture fixture) : base(fixture)
  {
  }

  [Fact]
  public async Task Test_Get_Users_MustReturnAllUsers()
  {
    var token = await GetLoginToken();


    var request = new HttpRequestMessage(HttpMethod.Get, "api/v1/users");
    request.Headers.Add("Authorization", "Bearer " + token);

    var response = await _httpClient.SendAsync(request);

    await response.Content.ReadAsStringAsync();

    response.EnsureSuccessStatusCode();

    ApiResult<List<User>>? responseList = await response.Content.ReadFromJsonAsync<ApiResult<List<User>>>();

    Assert.NotNull(responseList.Data);
    Assert.Single(responseList.Data);
    Assert.Equal("irwansyah@gmail.com", responseList.Data[0].Email);

  }
}