using Microsoft.AspNetCore.Mvc;

namespace Eko.Controllers;

[Controller]
[Route("Dashboard/[action]")]
public class DashboardController : Controller
{
    [HttpGet]
    public IActionResult Dashboard()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Profile()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ProfileEdit()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Faq()
    {
        return View();
    }

    [HttpGet]
    public IActionResult TermsConditions()
    {
        return View();
    }

    [HttpGet]
    public IActionResult PrivacyPolicy()
    {
        return View();
    }
}