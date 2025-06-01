using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eko.Controllers;

// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Controller]
[Route("Information/[action]")]
public class InformationController : Controller
{
    [HttpGet]
    public IActionResult StyleGuide()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult About()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Team()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Pricing()
    {
        return View();
    }

    [HttpGet]
    public IActionResult PricingDetails()
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

    [HttpGet]
    public IActionResult ComingSoon()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Maintenance()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Error()
    {
        return View();
    }
}