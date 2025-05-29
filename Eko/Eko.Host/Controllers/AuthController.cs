using Eko.Common.Cqrs;
using Eko.Features;
using Microsoft.AspNetCore.Mvc;

namespace Eko.Controllers;

[Controller]
[Route("Auth/[action]")]
public class AuthController : Controller
{
    private readonly ICommandHandler<PersonCommand> _handler;
    private readonly ILogger<AuthController> _logger;
    
    public AuthController(ICommandHandler<PersonCommand> handler, ILogger<AuthController> logger)
    {
        _handler = handler;
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
    public async Task<RedirectToActionResult> Post([FromForm] string name,
        [FromForm] string email, 
        [FromForm] string password)
    {
        _logger.LogInformation($"Full name: {name}");
        _logger.LogInformation($"Email: {email}");
        _logger.LogInformation($"Password: {password}");
        
        
        await _handler.Execute(new PersonCommand
        {
            fullName = name,
            email = email,
            password = password
        });
        return RedirectToAction("Index", "Home");
    }
}