using Microsoft.AspNetCore.Mvc;

namespace InventoryMgmt.Controllers;

public class ErrorController : Controller
{
    [Route("Error/Status")]
    public IActionResult Status(int code)
    {
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