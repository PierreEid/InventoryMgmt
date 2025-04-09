using Microsoft.AspNetCore.Mvc;

namespace InventoryMgmt.Controllers;

public class ErrorController : Controller
{
    private readonly ILogger<ErrorController> _logger;
    
    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }
    
    [Route("Error/Status")]
    public IActionResult Status(int code)
    {
        var path = HttpContext.Request.Path;
        var user = User.Identity?.IsAuthenticated == true ? User.Identity.Name : "Guest";
        _logger.LogWarning("Status Code {Code} on path {Path} | User: {User}", code, path, user);
        
        if (code == 404)
        {
            return View("NotFound");
        }

        return View("GenericError");
    }

    [Route("Error/ServerError")]
    public IActionResult ServerError()
    {
        return View("ServerError");
    }
}