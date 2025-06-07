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
    public async Task<IActionResult> Register([FromForm] string name,
        [FromForm] string email, 
        [FromForm] string password,
        [FromServices] ICommandHandler<CreatePersonCommand, string> handler)
    {
        try
        {
            var token = await handler.Execute(new CreatePersonCommand
            {
                fullName = name,
                email = email,
                password = password
            });

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(JwtOptions.ExpirationTime)
            });

            return RedirectToAction("Index", "Home");
        }
        catch (InvalidOperationException e)
        {
            if (Request.Headers["Accept"] == "application/json")
            {
                return Json(new { error = "Пользователь с таким email уже существует" });
            }
        
            TempData["ErrorMessage"] = "Пользователь с таким email уже существует";
            return RedirectToAction("Login");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password,
        [FromServices] ICommandHandler<VerifyPersonCommand, string> handler)
    {
        try
        {
            var token = await handler.Execute(new VerifyPersonCommand
            {
                Email = email,
                Password = password
            });
        
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(JwtOptions.ExpirationTime)
            });
        
            return RedirectToAction("Index", "Home");
        }
        catch (InvalidOperationException)
        {
            if (Request.Headers["Accept"] == "application/json")
            {
                return Json(new { error = "Неверный email или пароль" });
            }
        
            TempData["ErrorMessage"] = "Неверный email или пароль";
            return RedirectToAction("Login");
        }
    }

    [HttpGet]
    public async Task<RedirectToActionResult> Logout()
    {
        Response.Cookies.Delete("jwt");
        _logger.LogInformation("User is logged out");
        return RedirectToAction("SignIn", "Auth");
    }
}