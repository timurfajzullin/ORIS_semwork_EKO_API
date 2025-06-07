using Eko.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eko.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.AdminOrUser)]
[Controller]
[Route("Blog")]
public class BlogController : Controller
{
    [HttpGet]
    public IActionResult Blog()
    {
        return View();
    }

    [HttpGet("[action]")]
    public IActionResult BlogDetails()
    {
        return View();
    }
}