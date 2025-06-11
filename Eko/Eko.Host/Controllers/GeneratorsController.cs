using Eko.Auth;
using Eko.Common.Cqrs;
using Eko.Database.Entities;
using Eko.Features.AskDeepSeek;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eko.Controllers;

[Controller]
[Route("[controller]/[action]")]
// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.AdminOrUser)]
public class GeneratorsController : Controller
{
    private readonly ILogger<GeneratorsController> _logger;

    public GeneratorsController(ILogger<GeneratorsController> logger)
    {
        _logger = logger;
    }
    
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
    public IActionResult Text()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> AskDeepSeek([FromBody] string request,
        [FromServices] IQueryHandler<GetRequestAiQuery, string> handler)
    {
        var type = Request.Headers["Type"];
        try
        {
            _logger.LogInformation("AskDeepSeek");
            var response = await handler.Execute(new GetRequestAiQuery
            {
                Query = request,
                Type = type
            });
            return Ok(new { response });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<List<Chat>> GetChatsItems(
        [FromServices] IQueryHandler<GetChatsItemsQuery, List<Chat>> handler)
    {
        return await handler.Execute(new GetChatsItemsQuery());
    }
}