using Eko.Database;
using Microsoft.AspNetCore.Mvc;

namespace Eko.Controllers;

[Controller]
[Route("[controller]/[action]")]
public class SubscriptionController : Controller
{
    private readonly IEkoDbContext _context;
    private readonly ILogger<SubscriptionController> _logger;

    public SubscriptionController(IEkoDbContext context, ILogger<SubscriptionController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Subscription()
    {
        return View();
    }

    [HttpPost]
    public async Task<RedirectToActionResult> ChangePlan(int plan)
    {
        var email = Request.Headers["emailAddress"].FirstOrDefault();
        _logger.LogInformation(plan + "Ererere");
        _context.Person.FirstOrDefault(x => x.Email == email).Plan = plan;
        await _context.SaveChangesAsync();
        Response.Cookies.Delete("plan");
        Response.Cookies.Append("plan", plan.ToString());
        return RedirectToAction("Index", "Home");
    }
}