using Eko.Auth;
using Eko.Common.Cqrs;
using Eko.Features.AskDeepSeek;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eko.Controllers;

[Controller]
[Route("[controller]/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.AdminOrUser)]
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
    
    [HttpPost]
    public async Task<IActionResult> AskDeepSeek([FromBody] string request,
        [FromServices] IQueryHandler<GetRequestAiQuery, string> handler)
    {
        try
        {
            var response = await handler.Execute(new GetRequestAiQuery
            {
                Query = request
            });
            return Ok(new { response });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}