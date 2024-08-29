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
    await _serviceScope.ServiceProvider.GetService<AppDBContext>().Ships.ExecuteDeleteAsync();

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
    Assert.Equal(3, responseRetrieveData.data.Count);
    Assert.Equal("Ship-ABC-1", responseRetrieveData.data[0].name);

    await _serviceScope.ServiceProvider.GetService<AppDBContext>().Ships.ExecuteDeleteAsync();

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

}