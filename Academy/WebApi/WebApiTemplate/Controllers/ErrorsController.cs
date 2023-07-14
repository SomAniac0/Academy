using Microsoft.AspNetCore.Mvc;

namespace WebApiTemplate.Controllers;

public class ErrorsController : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/error")]
    public IActionResult Error()
    {
        return Problem();
    }
}