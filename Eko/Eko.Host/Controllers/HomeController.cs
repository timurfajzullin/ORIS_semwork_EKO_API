using System.Diagnostics;
using Eko.Auth;
using Eko.Common.Cqrs;
using Eko.Features.Notification;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.AdminOrUser)]
    [HttpGet("[action]")]
    public IActionResult ContactUs()
    {
        return View();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("[action]")]
    public async Task<RedirectToActionResult> ContactUsPost(
        [FromForm]string name,
        [FromForm]string email,
        [FromForm]string phone,
        [FromForm]string subject,
        [FromForm]string message,
        [FromServices] ICommandHandler<NotificationCommand> handler)
    {
        await handler.Execute(new NotificationCommand
        {
            Name = name,
            Email = email,
            Phone = phone,
            Subject = subject,
            Message = message
        });
        return RedirectToAction("Index", "Home");
    }
}