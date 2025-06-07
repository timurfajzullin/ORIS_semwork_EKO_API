using Eko.Auth.Jwt;
using Eko.Common.Cqrs;
using Eko.Database;
using Eko.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eko.Features;

public class VerifyPersonCommand : ICommand<string>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class VerifyPersonCommandHandler : ICommandHandler<VerifyPersonCommand, string>
{
    private readonly IEkoDbContext _dbContext;
    private readonly ILogger<VerifyPersonCommandHandler> _logger;
    private readonly IJwtTokenHandler _JwtTokenHandler;

    public VerifyPersonCommandHandler(IEkoDbContext dbContext, ILogger<VerifyPersonCommandHandler> logger, IJwtTokenHandler JwtTokenHandler)
    {
        _dbContext = dbContext;
        _logger = logger;
        _JwtTokenHandler = JwtTokenHandler;
    }
    
    public async Task<string> Execute(VerifyPersonCommand command)
    {
        var person = await VerifyPerson(command);
        _logger.LogInformation($"Пользователь {person.FirstName} {person.Email} верифицировался");
        var jwt = await _JwtTokenHandler.GenerateToken(person);
        return jwt;
    }

    private async Task<Database.Entities.Person?> VerifyPerson(VerifyPersonCommand command)
    {
        var person = await _dbContext.Person.FirstOrDefaultAsync(x => x.Email == command.Email && x.Password == command.Password);
        if (person == null) throw new InvalidOperationException("Invalid data");
        return person;
    }
}