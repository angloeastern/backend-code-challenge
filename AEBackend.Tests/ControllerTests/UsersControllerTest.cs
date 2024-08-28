

using System.Dynamic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AEBackend.Controllers;
using AEBackend.Controllers.Utils;
using AEBackend.DomainModels;
using AEBackend.DTOs;
using AEBackend.Repositories.RepositoryUsingEF;
using AEBackend.Tests.Fixtures;
using Docker.DotNet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;
using Respawn;
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

  [Theory]
  [InlineData("", "lastname", "role", "password", "abcd@gmail.com", "First name is required")]
  [InlineData("123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890", "lastname", "role", "password", "abcd@gmail.com", "The field FirstName must be a string with a maximum length of 100.")]
  [InlineData("firwstnae", "", "role", "password", "abcd@gmail.com", "Last name is required")]
  [InlineData("firwstnae", "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890", "role", "password", "abcd@gmail.com", "The field LastName must be a string with a maximum length of 100.")]
  [InlineData("firwstnae", "lastname", "", "password", "abcd@gmail.com", "Role is required")]
  [InlineData("firwstnae", "lastname", "1234567890123456789012345678901234567890", "password", "abcd@gmail.com", "The field Role must be a string with a maximum length of 20.")]
  [InlineData("firwstnae", "lastname", "role", "password", "abcd@gmail.com", "Role is not valid")]
  [InlineData("firwstnae", "lastname", "user", "", "abcd@gmail.com", "Password is required")]
  [InlineData("firwstnae", "lastname", "user", "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890", "abcd@gmail.com", "The field Password must be a string with a maximum length of 80.")]
  [InlineData("firwstnae", "lastname", "user", "asdadda", "", "Email is required, The Email field is not a valid e-mail address.")]
  [InlineData("firwstnae", "lastname", "user", "asdadda", "a@", "The Email field is not a valid e-mail address.")]
  [InlineData("firwstnae", "lastname", "user", "asdadda", "a@123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890.com", "The field Email must be a string with a maximum length of 100.")]

  public async Task Test_Create_Users_MakeSureRequestValidationsAreWorking(string firstName, string lastName,
      string role, string password, string email, string errorMessage)
  {
    var token = await GetLoginToken();


    var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/users");
    request.Headers.Add("Authorization", "Bearer " + token);

    var createUserRequest = $$"""
    {
      "firstName": "{{firstName}}",
      "lastName": "{{lastName}}",
      "role": "{{role}}",
      "password": "{{password}}",
      "email": "{{email}}"
    }
    """;

    _logger.LogDebug("createUserRequest:" + createUserRequest);

    request.Content = new StringContent(createUserRequest, Encoding.UTF8, "application/json");

    var response = await _httpClient.SendAsync(request);

    var responseString = await response.Content.ReadAsStringAsync();

    _logger.LogDebug($"responseString: {responseString}");

    dynamic responseData = JsonConvert.DeserializeObject<ExpandoObject>(responseString);

    _logger.LogDebug($"responseData: {responseData}");


    Assert.NotNull(responseData);
    Assert.Equal(false, responseData.isSuccess);
    Assert.Equal(errorMessage, responseData.error.message);
  }

  [Fact]
  public async Task Test_Create_Users_MustNotAllowDuplicatedEmail()
  {
    await _serviceScope.ServiceProvider.GetService<AppDBContext>().Users.Where(x => x.Email == "juki@gmail.com").ExecuteDeleteAsync();

    var token = await GetLoginToken();


    var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/users");
    request.Headers.Add("Authorization", "Bearer " + token);

    var createUserRequest = $$"""
    {
      "firstName": "juki",
      "lastName": "juki",
      "role": "User",
      "password": "Abcd1234!",
      "email": "juki@gmail.com"
    }
    """;

    _logger.LogDebug("createUserRequest:" + createUserRequest);

    request.Content = new StringContent(createUserRequest, Encoding.UTF8, "application/json");

    var response = await _httpClient.SendAsync(request);

    var responseString = await response.Content.ReadAsStringAsync();

    _logger.LogDebug($"responseString: {responseString}");

    dynamic responseData = JsonConvert.DeserializeObject<ExpandoObject>(responseString);

    _logger.LogDebug($"responseData: {responseData}");


    Assert.NotNull(responseData);
    Assert.Equal(true, responseData.isSuccess);


    //Send another request with the same email
    createUserRequest = $$"""
    {
      "firstName": "juki2",
      "lastName": "juki",
      "role": "User",
      "password": "Abcd1234!",
      "email": "juki@gmail.com"
    }
    """;
    request = new HttpRequestMessage(HttpMethod.Post, "api/v1/users");
    request.Headers.Add("Authorization", "Bearer " + token);
    request.Content = new StringContent(createUserRequest, Encoding.UTF8, "application/json");

    response = await _httpClient.SendAsync(request);

    responseString = await response.Content.ReadAsStringAsync();

    _logger.LogDebug($"responseString: {responseString}");

    responseData = JsonConvert.DeserializeObject<ExpandoObject>(responseString);

    Assert.NotNull(responseData);
    Assert.Equal(false, responseData.isSuccess);
    Assert.Equal("Email already taken", responseData.error.message);

    await _serviceScope.ServiceProvider.GetService<AppDBContext>().Users.Where(x => x.Email == "juki@gmail.com").ExecuteDeleteAsync();

  }

  [Fact]
  public async Task Test_Create_Users_MustStoredAllFieldsCorrectly()
  {
    await _serviceScope.ServiceProvider.GetService<AppDBContext>().Users.Where(x => x.Email == "juki@gmail.com").ExecuteDeleteAsync();

    var token = await GetLoginToken();


    var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/users");
    request.Headers.Add("Authorization", "Bearer " + token);

    var createUserRequest = $$"""
    {
      "firstName": "juki",
      "lastName": "juki",
      "role": "User",
      "password": "Abcd1234!",
      "email": "juki@gmail.com"
    }
    """;

    _logger.LogDebug("createUserRequest:" + createUserRequest);

    request.Content = new StringContent(createUserRequest, Encoding.UTF8, "application/json");

    var response = await _httpClient.SendAsync(request);

    var responseString = await response.Content.ReadAsStringAsync();

    _logger.LogDebug($"responseString: {responseString}");

    dynamic responseData = JsonConvert.DeserializeObject<ExpandoObject>(responseString);

    _logger.LogDebug($"responseData: {responseData}");


    Assert.NotNull(responseData);
    Assert.Equal(true, responseData.isSuccess);
    Assert.NotEmpty(responseData.data.id);
    Assert.Equal("juki", responseData.data.firstName);
    Assert.Equal("juki", responseData.data.lastName);
    Assert.Equal("juki@gmail.com", responseData.data.email);
    Assert.Equal("User", responseData.data.userRoles[0].role.name);

    await _serviceScope.ServiceProvider.GetService<AppDBContext>().Users.Where(x => x.Email == "juki@gmail.com").ExecuteDeleteAsync();

  }
}