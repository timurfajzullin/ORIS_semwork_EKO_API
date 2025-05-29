using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Eko.Controllers;
[Controller]
[Route("/")]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}