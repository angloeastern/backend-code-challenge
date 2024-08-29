using System.Dynamic;
using System.Text;
using AEBackend.DomainModels;
using AEBackend.Repositories.RepositoryUsingEF;
using AEBackend.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AEBackend.Tests.ControllerTests;

public class ShipsControllerTest : BaseControllerTest
{
  public ShipsControllerTest(CustomWebApplicationFactory factory) : base(factory)
  {
    // _logger.LogDebug("Container ID:" + fixture.ContainerId);
  }

  [Theory]
  [InlineData("", 0, -0.6, 31.2, "The Name field is required.")]
  [InlineData("123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890", 0, -0.6, 31.2, "The field Name must be a string with a maximum length of 100.")]
  [InlineData("Ship1", -1, -0.6, 31.2, "The field KnotVelocity must be between 0 and 60.")]
  [InlineData("Ship1", 61, -0.6, 31.2, "The field KnotVelocity must be between 0 and 60.")]
  [InlineData("Ship1", 20, -91, 31.2, "The field Lat must be between -90 and 90.")]
  [InlineData("Ship1", 20, 91, 31.2, "The field Lat must be between -90 and 90.")]
  [InlineData("Ship1", 20, 30, -181, "The field Long must be between -180 and 180.")]
  [InlineData("Ship1", 20, 30, 181, "The field Long must be between -180 and 180.")]
  public async Task Test_Create_Ship_MakeSureRequestValidationsAreWorking(
    string shipName,
    double knotVelocity,
    double lat,
    double longi,
    string errorMessage
  )
  {
    var token = await GetLoginToken();


    var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/ships");
    request.Headers.Add("Authorization", "Bearer " + token);

    var createShipRequest = $$"""
    {
      "name": "{{shipName}}",
      "knotVelocity": {{knotVelocity}},
      "lat": {{lat}},
      "long": {{longi}}      
    }
    """;

    request.Content = new StringContent(createShipRequest, Encoding.UTF8, "application/json");

    var response = await _httpClient.SendAsync(request);

    var responseString = await response.Content.ReadAsStringAsync();

    dynamic responseData = JsonConvert.DeserializeObject<ExpandoObject>(responseString);

    _logger.LogDebug($"*** Test_Create_Ship_MakeSureRequestValidationsAreWorking responseString: {responseString}");

    Assert.Equal(false, responseData.isSuccess);
    Assert.Equal(errorMessage, responseData.error.message);
  }

  [Fact]
  public async Task Test_Create_Ship_WithDuplicatedName_MustFailed()
  {
    await _serviceScope.ServiceProvider.GetService<AppDBContext>().Ships.Where(x => x.Name == "Ship1").ExecuteDeleteAsync();

    var token = await GetLoginToken();


    var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/ships");
    request.Headers.Add("Authorization", "Bearer " + token);

    var createShipRequest = $$"""
    {
      "name": "Ship1",
      "knotVelocity": 10,
      "lat": 20.2,
      "long": 10.5      
    }
    """;

    request.Content = new StringContent(createShipRequest, Encoding.UTF8, "application/json");

    var response = await _httpClient.SendAsync(request);

    var responseString = await response.Content.ReadAsStringAsync();

    dynamic responseData = JsonConvert.DeserializeObject<ExpandoObject>(responseString);

    _logger.LogDebug("$$$$$$$ responseString:" + responseString);

    Assert.Equal(true, responseData.isSuccess);

    //Make another request with the same ship name

    request = new HttpRequestMessage(HttpMethod.Post, "api/v1/ships");
    request.Headers.Add("Authorization", "Bearer " + token);

    createShipRequest = $$"""
    {
      "name": "Ship1",
      "knotVelocity": 30,
      "lat": 21.5,
      "long": 30.5      
    }
    """;

    request.Content = new StringContent(createShipRequest, Encoding.UTF8, "application/json");

    response = await _httpClient.SendAsync(request);

    responseString = await response.Content.ReadAsStringAsync();

    responseData = JsonConvert.DeserializeObject<ExpandoObject>(responseString);

    _logger.LogDebug("$$$$$$$ responseString:" + responseString);

    Assert.Equal(false, responseData.isSuccess);
    Assert.Equal("Ship Name already taken", responseData.error.message);


    await _serviceScope.ServiceProvider.GetService<AppDBContext>().Ships.Where(x => x.Name == "Ship1").ExecuteDeleteAsync();
  }

  [Fact]
  public async Task Test_Create_Ship_MustStoredFieldsCorrecly()
  {
    await _serviceScope.ServiceProvider.GetService<AppDBContext>().Ships.Where(x => x.Name == "Ship1").ExecuteDeleteAsync();

    var token = await GetLoginToken();


    var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/ships");
    request.Headers.Add("Authorization", "Bearer " + token);

    var createShipRequest = $$"""
    {
      "name": "Ship1",
      "knotVelocity": 10,
      "lat": 20.2,
      "long": 10.5      
    }
    """;

    request.Content = new StringContent(createShipRequest, Encoding.UTF8, "application/json");

    var response = await _httpClient.SendAsync(request);

    var responseString = await response.Content.ReadAsStringAsync();

    dynamic responseData = JsonConvert.DeserializeObject<ExpandoObject>(responseString);

    _logger.LogDebug("$$$$$$$ responseString:" + responseString);

    Assert.Equal(true, responseData.isSuccess);
    Assert.Equal(10, responseData.data.velocity.value);
    Assert.Equal("Knot", responseData.data.velocity.unitName);
    Assert.Equal(20.2, responseData.data.lat);
    Assert.Equal(10.5, responseData.data.longi);

    await _serviceScope.ServiceProvider.GetService<AppDBContext>().Ships.Where(x => x.Name == "Ship1").ExecuteDeleteAsync();
  }

  [Fact]
  public async Task Test_Get_Ships_MustReturnAllShips()
  {
    _serviceScope.ServiceProvider.GetService<AppDBContext>().Ships.Where(x => x.Name.StartsWith("Ship-ABC")).ExecuteDelete();

    var token = await GetLoginToken();


    for (var i = 1; i <= 3; i++)
    {
      var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/ships");
      request.Headers.Add("Authorization", "Bearer " + token);

      var createShipRequest = $$"""
      {
        "name": "Ship-ABC-{{i}}",
        "knotVelocity": 10,
        "lat": 20.2,
        "long": 10.5      
      }
      """;

      request.Content = new StringContent(createShipRequest, Encoding.UTF8, "application/json");

      var response = await _httpClient.SendAsync(request);

      var responseString = await response.Content.ReadAsStringAsync();

      dynamic responseData = JsonConvert.DeserializeObject<ExpandoObject>(responseString);

      _logger.LogDebug("$$$$$$$ responseString:" + responseString);

      Assert.Equal(true, responseData.isSuccess);
    }

    //Retrieve all ships
    var requestRetrieve = new HttpRequestMessage(HttpMethod.Get, "api/v1/ships");
    requestRetrieve.Headers.Add("Authorization", "Bearer " + token);

    var responseRetrieve = await _httpClient.SendAsync(requestRetrieve);

    var responseRetieveString = await responseRetrieve.Content.ReadAsStringAsync();
    _logger.LogDebug("$$$$$$$ responseRetrieveString:" + responseRetieveString);

    dynamic responseRetrieveData = JsonConvert.DeserializeObject<ExpandoObject>(responseRetieveString);



    Assert.Equal(true, responseRetrieveData.isSuccess);
    Assert.True(responseRetrieveData.data.Count >= 3);

    List<dynamic> existingShips = responseRetrieveData.data;
    Assert.True(existingShips.Select(x => x.name).Contains("Ship-ABC-1"));
    Assert.True(existingShips.Select(x => x.name).Contains("Ship-ABC-2"));
    Assert.True(existingShips.Select(x => x.name).Contains("Ship-ABC-3"));

    _serviceScope.ServiceProvider.GetService<AppDBContext>().Ships.Where(x => x.Name.StartsWith("Ship-ABC")).ExecuteDelete();

  }

  [Fact]
  public async Task Test_Update_Ship_Velocity_MustStoredFieldsCorrecly()
  {
    await _serviceScope.ServiceProvider.GetService<AppDBContext>().Ships.Where(x => x.Name == "Ship1").ExecuteDeleteAsync();

    var token = await GetLoginToken();


    var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/ships");
    request.Headers.Add("Authorization", "Bearer " + token);

    var createShipRequest = $$"""
    {
      "name": "Ship1",
      "knotVelocity": 10,
      "lat": 20.2,
      "long": 10.5      
    }
    """;

    request.Content = new StringContent(createShipRequest, Encoding.UTF8, "application/json");

    var response = await _httpClient.SendAsync(request);

    var responseString = await response.Content.ReadAsStringAsync();

    dynamic responseData = JsonConvert.DeserializeObject<ExpandoObject>(responseString);

    _logger.LogDebug("$$$$$$$ responseString:" + responseString);

    Assert.Equal(true, responseData.isSuccess);

    //Update ship velocity
    request = new HttpRequestMessage(HttpMethod.Put, $"api/v1/ships/{responseData.data.id}/Velocity");
    request.Headers.Add("Authorization", "Bearer " + token);

    _logger.LogDebug("%%%% update uri: " + request.RequestUri);

    var updateShipVelocityRequest = $$"""
    {      
      "knotVelocity": 30
    }
    """;

    _logger.LogDebug("%%%%%% updateShipVelocityRequest:" + updateShipVelocityRequest);

    request.Content = new StringContent(updateShipVelocityRequest, Encoding.UTF8, "application/json");

    response = await _httpClient.SendAsync(request);

    responseString = await response.Content.ReadAsStringAsync();

    responseData = JsonConvert.DeserializeObject<ExpandoObject>(responseString);

    _logger.LogDebug("$$$$$$$ responseUpdateString:" + responseString);

    Assert.Equal(true, responseData.isSuccess);
    Assert.Equal(30, responseData.data.velocity.value);
    Assert.Equal("Knot", responseData.data.velocity.unitName);
    Assert.Equal(20.2, responseData.data.lat);
    Assert.Equal(10.5, responseData.data.longi);

    await _serviceScope.ServiceProvider.GetService<AppDBContext>().Ships.Where(x => x.Name == "Ship1").ExecuteDeleteAsync();
  }

  [Theory]
  [InlineData(-1, "The field KnotVelocity must be between 0 and 60.")]
  [InlineData(61, "The field KnotVelocity must be between 0 and 60.")]
  public async Task Test_Update_Ship_Velocity_MustValidateRequestsCorrectly(double knotVelocity, string errorMessage)
  {
    await _serviceScope.ServiceProvider.GetService<AppDBContext>().Ships.Where(x => x.Name == "Ship1").ExecuteDeleteAsync();

    var token = await GetLoginToken();


    var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/ships");
    request.Headers.Add("Authorization", "Bearer " + token);

    var createShipRequest = $$"""
    {
      "name": "Ship1",
      "knotVelocity": 10,
      "lat": 20.2,
      "long": 10.5      
    }
    """;

    request.Content = new StringContent(createShipRequest, Encoding.UTF8, "application/json");

    var response = await _httpClient.SendAsync(request);

    var responseString = await response.Content.ReadAsStringAsync();

    dynamic responseData = JsonConvert.DeserializeObject<ExpandoObject>(responseString);

    _logger.LogDebug("$$$$$$$ responseString:" + responseString);

    Assert.Equal(true, responseData.isSuccess);

    //Update ship velocity
    request = new HttpRequestMessage(HttpMethod.Put, $"api/v1/ships/{responseData.data.id}/Velocity");
    request.Headers.Add("Authorization", "Bearer " + token);

    _logger.LogDebug("%%%% update uri: " + request.RequestUri);

    var updateShipVelocityRequest = $$"""
    {      
      "knotVelocity": {{knotVelocity}}
    }
    """;

    _logger.LogDebug("%%%%%% updateShipVelocityRequest:" + updateShipVelocityRequest);

    request.Content = new StringContent(updateShipVelocityRequest, Encoding.UTF8, "application/json");

    response = await _httpClient.SendAsync(request);

    responseString = await response.Content.ReadAsStringAsync();

    responseData = JsonConvert.DeserializeObject<ExpandoObject>(responseString);

    _logger.LogDebug("$$$$$$$ responseUpdateString:" + responseString);

    Assert.Equal(false, responseData.isSuccess);
    Assert.Equal(errorMessage, responseData.error.message);

    await _serviceScope.ServiceProvider.GetService<AppDBContext>().Ships.Where(x => x.Name == "Ship1").ExecuteDeleteAsync();
  }


  [Fact]
  [Trait("TraitName", "Filtered")]
  public async Task Test_See_Ships_Unassigned_MustReturnedCorrectly()
  {
    var dbContext = _serviceScope.ServiceProvider.GetService<AppDBContext>();
    var cleanUpUsers = dbContext.Users.Include(u => u.UserShips).Where(x => x.Email.StartsWith("ronnisua")).ToList();
    dbContext.RemoveRange(cleanUpUsers);
    var cleanUpShips = dbContext.Ships.Include(x => x.UserShips).Where(x => x.Name.StartsWith("ShipSUA")).ToList();
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
              "email": "ronnisua{{i}}@gmail.com"
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
              "name": "ShipSUA{{i}}",
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

    //Retrieved unassigned ships
    var requestUnassignedShips = new HttpRequestMessage(HttpMethod.Get, $"api/v1/ships/unassigneds");

    _logger.LogDebug("%%% requestUnassignedShips.RequestUri:" + requestUnassignedShips.RequestUri);

    requestUnassignedShips.Headers.Add("Authorization", "Bearer " + token);

    var responseUnassignedShips = await _httpClient.SendAsync(requestUnassignedShips);

    var responseStringUnassignedShips = await responseUnassignedShips.Content.ReadAsStringAsync();


    _logger.LogDebug($"responseStringUnassignedShips: {responseStringUnassignedShips}");

    dynamic responseUnassignedShipsJson = JsonConvert.DeserializeObject<ExpandoObject>(responseStringUnassignedShips);

    Assert.Equal(true, responseUnassignedShipsJson.isSuccess);
    Assert.Equal(2, responseUnassignedShipsJson.data.Count);

    List<dynamic> updatedShips = responseUnassignedShipsJson.data;

    Assert.True(updatedShips.Select(x => x.name).ToList().Contains("ShipSUA4"));
    Assert.True(updatedShips.Select(x => x.name).ToList().Contains("ShipSUA5"));

    cleanUpUsers = dbContext.Users.Include(u => u.UserShips).Where(x => x.Email.StartsWith("ronnisua")).ToList();
    dbContext.RemoveRange(cleanUpUsers);
    cleanUpShips = dbContext.Ships.Include(x => x.UserShips).Where(x => x.Name.StartsWith("ShipSUA")).ToList();
    dbContext.RemoveRange(cleanUpShips);
    dbContext.SaveChanges();
  }

  [Theory]
  [Trait("TraitName", "Filtered")]
  [InlineData("Bandung", -6.9796439391129015, 107.72736494836637, "Tanjung Priok")]
  [InlineData("Cisauk", -6.337957856734018, 106.64177845684021, "Jakarta, Java")]
  [InlineData("Karang Anyar", -7.603521654106759, 111.01227712457657, "Semarang")]
  public async Task Test_NearestPort_MustReturnCorrectInformation(string areaName, double lat, double longi, string nearestPortName)
  {
    await _serviceScope.ServiceProvider.GetService<AppDBContext>().Ships.Where(x => x.Name == "ShipNearest").ExecuteDeleteAsync();

    var token = await GetLoginToken();


    var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/ships");
    request.Headers.Add("Authorization", "Bearer " + token);


    var createShipRequest = $$"""
    {
      "name": "ShipNearest",
      "knotVelocity": 10,
      "lat": {{lat}},
      "long": {{longi}}     
    }
    """;

    request.Content = new StringContent(createShipRequest, Encoding.UTF8, "application/json");

    var response = await _httpClient.SendAsync(request);

    var responseString = await response.Content.ReadAsStringAsync();

    dynamic responseData = JsonConvert.DeserializeObject<ExpandoObject>(responseString);

    _logger.LogDebug("$$$$$$$ responseString:" + responseString);

    Assert.Equal(true, responseData.isSuccess);

    //Get nearest port
    request = new HttpRequestMessage(HttpMethod.Get, $"api/v1/ships/{responseData.data.id}/NearestPort");

    _logger.LogDebug("request URi:" + request.RequestUri);
    request.Headers.Add("Authorization", "Bearer " + token);

    response = await _httpClient.SendAsync(request);

    responseString = await response.Content.ReadAsStringAsync();
    _logger.LogDebug("$$$$$$$ responseStringNearest:" + responseString);

    responseData = JsonConvert.DeserializeObject<ExpandoObject>(responseString);



    Assert.Equal(true, responseData.isSuccess);
    Assert.Equal(nearestPortName, responseData.data.port.name);

    await _serviceScope.ServiceProvider.GetService<AppDBContext>().Ships.Where(x => x.Name == "ShipNearest").ExecuteDeleteAsync();
  }

}