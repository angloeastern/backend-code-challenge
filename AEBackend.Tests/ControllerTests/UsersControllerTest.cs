

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

public class UsersControllerTest : BaseControllerTest
{

  public UsersControllerTest(CustomWebApplicationFactory factory) : base(factory)
  {
    // _logger.LogDebug("Container ID:" + fixture.ContainerId);
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
    Assert.True(responseList.Data.Count >= 1);

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


    Assert.NotNull(responseData);
    Assert.Equal(true, responseData.isSuccess);
    Assert.NotEmpty(responseData.data.id);
    Assert.Equal("juki", responseData.data.firstName);
    Assert.Equal("juki", responseData.data.lastName);
    Assert.Equal("juki@gmail.com", responseData.data.email);
    Assert.Equal(1, responseData.data.userRoles.Count);
    Assert.Equal("User", responseData.data.userRoles[0].role.name);

    await _serviceScope.ServiceProvider.GetService<AppDBContext>().Users.Where(x => x.Email == "juki@gmail.com").ExecuteDeleteAsync();


  }

  [Fact]
  public async Task Test_Update_Ships_Assigned_To_Users_MustUpdateCorrectly()
  {

    var dbContext = _serviceScope.ServiceProvider.GetService<AppDBContext>();
    var cleanUpUsers = dbContext.Users.Include(u => u.UserShips).Where(x => x.Email.StartsWith("juki-update-ship")).ToList();
    dbContext.RemoveRange(cleanUpUsers);
    var cleanUpShips = dbContext.Ships.Include(x => x.UserShips).Where(x => x.Name.StartsWith("ShipUpdate")).ToList();
    dbContext.RemoveRange(cleanUpShips);
    dbContext.SaveChanges();






    var token = await GetLoginToken();

    //Create two new users
    for (int i = 1; i <= 2; i++)
    {
      var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/users");
      request.Headers.Add("Authorization", "Bearer " + token);

      var createUserRequest = $$"""
        {
          "firstName": "User{{i}}",
          "lastName": "juki",
          "role": "User",
          "password": "Abcd1234!",
          "email": "juki-update-ship{{i}}@gmail.com"
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
    }

    //Create 5 new ships
    List<dynamic> createdShips = [];
    for (int i = 1; i <= 5; i++)
    {
      var requestCreateShip = new HttpRequestMessage(HttpMethod.Post, "api/v1/ships");
      requestCreateShip.Headers.Add("Authorization", "Bearer " + token);

      var createShipRequest = $$"""
        {
          "name": "ShipUpdate{{i}}",
          "knotVelocity": 10,
          "lat": 20.2,
          "long": 10.5      
        }
        """;


      requestCreateShip.Content = new StringContent(createShipRequest, Encoding.UTF8, "application/json");

      var responseCreateShip = await _httpClient.SendAsync(requestCreateShip);

      var responseStringCreateShip = await responseCreateShip.Content.ReadAsStringAsync();

      _logger.LogDebug($"responseStringCreateShip: {responseStringCreateShip}");

      dynamic responseDataCreateShip = JsonConvert.DeserializeObject<ExpandoObject>(responseStringCreateShip);

      _logger.LogDebug($"responseDataCreateShip: {responseDataCreateShip}");

      createdShips.Add(responseDataCreateShip.data);


      Assert.NotNull(responseDataCreateShip);
      Assert.Equal(true, responseDataCreateShip.isSuccess);
    }

    //Retrieve all created users
    var requestAllUsers = new HttpRequestMessage(HttpMethod.Get, "api/v1/users");
    requestAllUsers.Headers.Add("Authorization", "Bearer " + token);

    var responseAllUsers = await _httpClient.SendAsync(requestAllUsers);

    var responseStringAllUsers = await responseAllUsers.Content.ReadAsStringAsync();

    _logger.LogDebug($"responseStringAllUsers: {responseStringAllUsers}");

    dynamic allUsersResponse = JsonConvert.DeserializeObject<ExpandoObject>(responseStringAllUsers);

    _logger.LogDebug($"allUsersResponse: {allUsersResponse}");

    Assert.Equal(true, allUsersResponse.isSuccess);

    //Assign each users two ships
    for (int i = 1; i <= 2; i++)
    {
      dynamic currentUser = allUsersResponse.data[i - 1];
      List<string> chosenShipsIds = [];
      List<dynamic> chosenShips = [];
      for (int j = 1; j <= 2; j++)
      {
        dynamic chosenShip = createdShips[(j - 1) * i];

        _logger.LogDebug("%%% chosenShip:" + (string)chosenShip.id);

        chosenShipsIds.Add(chosenShip.id);
        chosenShips.Add(chosenShip);
      }

      var requestAssignShip = new HttpRequestMessage(HttpMethod.Put, $"api/v1/users/{allUsersResponse.data[i - 1].id}/Ships");

      _logger.LogDebug("%%% requestAssignShip.RequestUri:" + requestAssignShip.RequestUri);

      requestAssignShip.Headers.Add("Authorization", "Bearer " + token);

      var updateShipRequest = $$"""
        {
          "shipdIds": {{JsonConvert.SerializeObject(chosenShipsIds)}}
        }
        """;

      _logger.LogDebug("%%% updateShipRequest:" + updateShipRequest);

      requestAssignShip.Content = new StringContent(updateShipRequest, Encoding.UTF8, "application/json");


      var responseAssignShip = await _httpClient.SendAsync(requestAssignShip);

      var responsStringAssignShip = await responseAssignShip.Content.ReadAsStringAsync();


      _logger.LogDebug($"responsStringAssignShip: {responsStringAssignShip}");

      dynamic assignShipResponse = JsonConvert.DeserializeObject<ExpandoObject>(responsStringAssignShip);

      Assert.Equal(true, assignShipResponse.isSuccess);
      Assert.Equal(2, assignShipResponse.data.userShips.Count);

      Assert.True(chosenShipsIds.Contains(assignShipResponse.data.userShips[0].shipId));
      Assert.True(chosenShipsIds.Contains(assignShipResponse.data.userShips[1].shipId));
    }

    //Update assigned ships of user[1]
    var requestUpdateAssignShip = new HttpRequestMessage(HttpMethod.Put, $"api/v1/users/{allUsersResponse.data[1].id}/Ships");

    _logger.LogDebug("%%% requestUpdateAssignShip.RequestUri:" + requestUpdateAssignShip.RequestUri);

    requestUpdateAssignShip.Headers.Add("Authorization", "Bearer " + token);

    var requestUpdateShipRequest = $$"""
        {
          "shipdIds": ["{{createdShips[0].id}}", "{{createdShips[4].id}}", "{{createdShips[1].id}}"]
        }
        """;

    _logger.LogDebug("%%% requestUpdateShipRequest:" + requestUpdateShipRequest);

    requestUpdateAssignShip.Content = new StringContent(requestUpdateShipRequest, Encoding.UTF8, "application/json");


    var responseUpdateAssignShips = await _httpClient.SendAsync(requestUpdateAssignShip);

    var responsStringAssignShips = await responseUpdateAssignShips.Content.ReadAsStringAsync();


    _logger.LogDebug($"responsStringAssignShips: {responsStringAssignShips}");

    dynamic updateAssignShipsResponse = JsonConvert.DeserializeObject<ExpandoObject>(responsStringAssignShips);

    Assert.Equal(true, updateAssignShipsResponse.isSuccess);
    Assert.Equal(3, updateAssignShipsResponse.data.userShips.Count);

    List<dynamic> updatedShips = updateAssignShipsResponse.data.userShips;

    Assert.True(updatedShips.Select(x => x.shipId).ToList().Contains(createdShips[0].id));
    Assert.True(updatedShips.Select(x => x.shipId).ToList().Contains(createdShips[1].id));
    Assert.True(updatedShips.Select(x => x.shipId).ToList().Contains(createdShips[4].id));


    cleanUpUsers = dbContext.Users.Include(u => u.UserShips).Where(x => x.Email.StartsWith("juki-update-ship")).ToList();
    dbContext.RemoveRange(cleanUpUsers);
    cleanUpShips = dbContext.Ships.Include(x => x.UserShips).Where(x => x.Name.StartsWith("ShipUpdate")).ToList();
    dbContext.RemoveRange(cleanUpShips);
    dbContext.SaveChanges();

  }


  [Fact]

  public async Task Test_See_Ships_Assigned_To_SpecificUser_MustReturnedCorrectly()
  {
    var dbContext = _serviceScope.ServiceProvider.GetService<AppDBContext>();
    var cleanUpUsers = dbContext.Users.Include(u => u.UserShips).Where(x => x.Email.StartsWith("ronni")).ToList();
    dbContext.RemoveRange(cleanUpUsers);
    var cleanUpShips = dbContext.Ships.Include(x => x.UserShips).Where(x => x.Name.StartsWith("ShipSSU")).ToList();
    dbContext.RemoveRange(cleanUpShips);
    dbContext.SaveChanges();



    var token = await GetLoginToken();

    //Create two new users
    for (int i = 1; i <= 2; i++)
    {
      var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/users");
      request.Headers.Add("Authorization", "Bearer " + token);

      var createUserRequest = $$"""
            {
              "firstName": "User{{i}}",
              "lastName": "ronni",
              "role": "User",
              "password": "Abcd1234!",
              "email": "ronni{{i}}@gmail.com"
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
    }

    //Create 5 new ships
    List<dynamic> createdShips = [];
    for (int i = 1; i <= 5; i++)
    {
      var requestCreateShip = new HttpRequestMessage(HttpMethod.Post, "api/v1/ships");
      requestCreateShip.Headers.Add("Authorization", "Bearer " + token);

      var createShipRequest = $$"""
            {
              "name": "ShipSSU{{i}}",
              "knotVelocity": 10,
              "lat": 20.2,
              "long": 10.5      
            }
            """;


      requestCreateShip.Content = new StringContent(createShipRequest, Encoding.UTF8, "application/json");

      var responseCreateShip = await _httpClient.SendAsync(requestCreateShip);

      var responseStringCreateShip = await responseCreateShip.Content.ReadAsStringAsync();

      _logger.LogDebug($"responseStringCreateShip: {responseStringCreateShip}");

      dynamic responseDataCreateShip = JsonConvert.DeserializeObject<ExpandoObject>(responseStringCreateShip);

      _logger.LogDebug($"responseDataCreateShip: {responseDataCreateShip}");

      createdShips.Add(responseDataCreateShip.data);


      Assert.NotNull(responseDataCreateShip);
      Assert.Equal(true, responseDataCreateShip.isSuccess);
    }

    //Retrieve all created users
    var requestAllUsers = new HttpRequestMessage(HttpMethod.Get, "api/v1/users");
    requestAllUsers.Headers.Add("Authorization", "Bearer " + token);

    var responseAllUsers = await _httpClient.SendAsync(requestAllUsers);

    var responseStringAllUsers = await responseAllUsers.Content.ReadAsStringAsync();

    _logger.LogDebug($"responseStringAllUsers: {responseStringAllUsers}");

    dynamic allUsersResponse = JsonConvert.DeserializeObject<ExpandoObject>(responseStringAllUsers);

    _logger.LogDebug($"allUsersResponse: {allUsersResponse}");

    Assert.Equal(true, allUsersResponse.isSuccess);

    //Assign each users two ships
    for (int i = 1; i <= 2; i++)
    {
      dynamic currentUser = allUsersResponse.data[i - 1];
      List<string> chosenShipsIds = [];
      List<dynamic> chosenShips = [];
      for (int j = 1; j <= 2; j++)
      {
        dynamic chosenShip = createdShips[(j - 1) * i];

        _logger.LogDebug("%%% chosenShip:" + (string)chosenShip.id);

        chosenShipsIds.Add(chosenShip.id);
        chosenShips.Add(chosenShip);
      }

      var requestAssignShip = new HttpRequestMessage(HttpMethod.Put, $"api/v1/users/{allUsersResponse.data[i - 1].id}/Ships");

      _logger.LogDebug("%%% requestAssignShip.RequestUri:" + requestAssignShip.RequestUri);

      requestAssignShip.Headers.Add("Authorization", "Bearer " + token);

      var updateShipRequest = $$"""
            {
              "shipdIds": {{JsonConvert.SerializeObject(chosenShipsIds)}}
            }
            """;

      _logger.LogDebug("%%% updateShipRequest:" + updateShipRequest);

      requestAssignShip.Content = new StringContent(updateShipRequest, Encoding.UTF8, "application/json");


      var responseAssignShip = await _httpClient.SendAsync(requestAssignShip);

      var responsStringAssignShip = await responseAssignShip.Content.ReadAsStringAsync();


      _logger.LogDebug($"responsStringAssignShip: {responsStringAssignShip}");

      dynamic assignShipResponse = JsonConvert.DeserializeObject<ExpandoObject>(responsStringAssignShip);

      Assert.Equal(true, assignShipResponse.isSuccess);
      Assert.Equal(2, assignShipResponse.data.userShips.Count);
      Assert.Equal(chosenShipsIds[0], assignShipResponse.data.userShips[0].shipId);
      Assert.Equal(chosenShipsIds[1], assignShipResponse.data.userShips[1].shipId);
    }

    //Retrieved assigned ships
    var requestAssignedShips = new HttpRequestMessage(HttpMethod.Get, $"api/v1/users/{allUsersResponse.data[1].id}/Ships");

    _logger.LogDebug("%%% requestAssignedShips.RequestUri:" + requestAssignedShips.RequestUri);

    requestAssignedShips.Headers.Add("Authorization", "Bearer " + token);

    var responseAssignedShips = await _httpClient.SendAsync(requestAssignedShips);

    var responsStringAssignedShips = await responseAssignedShips.Content.ReadAsStringAsync();


    _logger.LogDebug($"responsStringAssignedShips: {responsStringAssignedShips}");

    dynamic responseUserAssignedShips = JsonConvert.DeserializeObject<ExpandoObject>(responsStringAssignedShips);

    Assert.Equal(true, responseUserAssignedShips.isSuccess);
    Assert.Equal(2, responseUserAssignedShips.data.Count);

    List<dynamic> updatedShips = responseUserAssignedShips.data;

    Assert.True(updatedShips.Select(x => x.id).ToList().Contains(createdShips[0].id));
    Assert.True(updatedShips.Select(x => x.id).ToList().Contains(createdShips[2].id));

    cleanUpUsers = dbContext.Users.Include(u => u.UserShips).Where(x => x.Email.StartsWith("ronni")).ToList();
    dbContext.RemoveRange(cleanUpUsers);
    cleanUpShips = dbContext.Ships.Include(x => x.UserShips).Where(x => x.Name.StartsWith("ShipSSU")).ToList();
    dbContext.RemoveRange(cleanUpShips);
    dbContext.SaveChanges();
  }
}