using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Eko.Controllers;


[ApiController]
[Route("")]
public class HomeController : Controller
{
    public IActionResult Get()
    {
        return View();
    }
}