using Eko.Auth;
using Eko.Common.Cqrs;
using Eko.Database.Entities;
using Eko.Features.AdminPanel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eko.Controllers;

[Controller]
[Route("[controller]/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policy.Admin)]
public class AdminPanelController : Controller
{
    [HttpGet]
    public IActionResult AdminPanel()
    {
        return View();
    }

    [HttpGet]
    public async Task<List<Person>> GetPersons(
        [FromServices] IQueryHandler<GetPersonsQuery, List<Person>> handler)
    {
        return await handler.Execute(new GetPersonsQuery());
    }

    [HttpDelete]
    public async Task DeletePerson(
        [FromQuery]string email,
        [FromServices] ICommandHandler<DeletePersonCommand> handler)
    {
        await handler.Execute(new DeletePersonCommand
        {
            Email = email
        });
    }

    [HttpPost]
    public async Task RegisterPerson(
        [FromForm] string email,
        [FromForm] string firstName,
        [FromForm] string lastName,
        [FromForm] bool isAdmin,
        [FromForm] string password,
        [FromServices] ICommandHandler<RegisterPersonCommand> handler)
    {
        await handler.Execute(new RegisterPersonCommand
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            IsAdmin = isAdmin,
            Password = password
        });
    }


    [HttpGet]
    public async Task<Profile> GetProfile(
        [FromQuery] string email,
        [FromServices] IQueryHandler<GetProfileQuery, Profile> handler)
    {
        return await handler.Execute(new GetProfileQuery
        {
            Email = email
        });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile(
        [FromQuery] string email,
        [FromForm] string? birthDate,
        [FromForm] string? experience,
        [FromForm] string? bio,
        [FromForm] string? skills,
        [FromForm] string? specialization,
        [FromServices] ICommandHandler<UpdateProfileCommand> handler)
    {
        try
        {
            var command = new UpdateProfileCommand
            {
                Email = email,
                BirthDate = birthDate,
                Experience = experience,
                Bio = bio,
                Skills = skills,
                Specialization = specialization
            };

            await handler.Execute(command);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}