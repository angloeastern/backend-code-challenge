using Microsoft.AspNetCore.Mvc;

namespace AEBackend.Controllers.Utils;

[ApiController]
public class ApplicationController : ControllerBase
{
  protected List<string> GetModelStateErrors()
  {
    var er = ModelState.Values.Select(x => x.Errors.ToList()).ToList();

    List<string> errorMessages = [];
    er.ForEach(x => x.ForEach(y =>
    {
      errorMessages.Add(y.ErrorMessage);
    }));

    return errorMessages;
  }
}