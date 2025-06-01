using Eko.Auth.Jwt;
using Eko.Common.Cqrs;
using Eko.Features;
using Microsoft.AspNetCore.Mvc;

namespace Eko.Controllers;

[Controller]
[Route("Auth/[action]")]
public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    
    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<RedirectToActionResult> Register([FromForm] string name,
        [FromForm] string email, 
        [FromForm] string password,
        [FromServices] ICommandHandler<CreatePersonCommand, string> handler)
    {
        var token = await handler.Execute(new CreatePersonCommand
        {
            fullName = name,
            email = email,
            password = password
        });

        Response.Cookies.Append("jwt", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true, 
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(JwtOptions.ExpirationTime)
        });

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<RedirectToActionResult> Login([FromForm] string email, [FromForm] string password,
        [FromServices] ICommandHandler<VerifyPersonCommand, string> handler)
    {
        var token = await handler.Execute(new VerifyPersonCommand
        {
            Email = email,
            Password = password
        });
        
        Response.Cookies.Append("jwt", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // Для HTTPS
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(JwtOptions.ExpirationTime)
        });
        
        return RedirectToAction("Index", "Home");
    }
}