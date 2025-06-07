using Eko.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Eko.Auth;
using Eko.Common.Cqrs;
using Eko.Database;
using Eko.Features.Person;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace Eko.Controllers;

[Controller]
[Route("[controller]/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.AdminOrUser)]
public class PersonController : Controller
{
    private readonly ILogger<PersonController> _logger;

    public PersonController(ILogger<PersonController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<Profile> TakePerson(
        [FromServices] IQueryHandler<GetPersonInfoQuery, Profile> handler)
    {

        var email = Request.Headers["emailAddress"].FirstOrDefault();
        if (string.IsNullOrEmpty(email))
        {
            return null;
        }
        var person = await handler.Execute(new GetPersonInfoQuery
        {
            Email = email
        });

        return person;
    }

    [HttpPost]
    public async Task SaveInfo(
        [FromForm] string phone, 
        [FromForm] string dateOfBirth,
        [FromForm] string experience,
        [FromForm] string biography,
        [FromForm] string skills,
        [FromForm] string specialization, 
        [FromServices] ICommandHandler<ChangePersonInfoCommand> handler)
    {
        _logger.LogInformation($"Received data: Phone={phone}, Skills={skills}, Specialization={specialization}, etc.");
        var email = Request.Headers["emailAddress"].FirstOrDefault();
        await handler.Execute(new ChangePersonInfoCommand
        {
            Email = email,
            Phone = phone,
            Experience = experience,
            Biography = biography,
            Skills = skills,
            DataOfBirth = dateOfBirth,
            Specialization = specialization
        });
    }
}