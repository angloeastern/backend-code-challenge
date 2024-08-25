

using AEBackend.Controllers;
using AEBackend.DTOs;
using Xunit.Abstractions;

namespace AEBackend.Tests;

public class UsersControllerTest
{

  private readonly ITestOutputHelper output;

  public UsersControllerTest(ITestOutputHelper output)
  {
    this.output = output;
  }
  [Fact]
  public void Test1()
  {
    var controller = new UsersController();

    var dtos = controller.Get();

    Assert.Equal(2, dtos.Length);


  }
}