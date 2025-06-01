using Microsoft.AspNetCore.Mvc;

namespace Eko.Controllers;

[Controller]
[Route("[controller]/[action]")]
public class GeneratorsController : Controller
{
    [HttpGet]
    public IActionResult Code()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Email()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Image()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Text()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Video()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Website()
    {
        return View();
    }
}