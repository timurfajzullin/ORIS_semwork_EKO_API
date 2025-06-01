using System.Windows.Input;
using Eko.Auth.Jwt;
using Eko.Common.Cqrs;
using Eko.Database;
using Eko.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ICommand = Eko.Common.Cqrs.ICommand;

namespace Eko.Features;

public class CreatePersonCommand : ICommand<string>
{
    public string fullName {get; set;}
    public string email {get; set;}
    public string password {get; set;}
}

public class CreatePersonCommandHandler : ICommandHandler<CreatePersonCommand, string>
{
    private readonly IEkoDbContext _dbContext;
    private readonly ILogger _logger;
    private readonly IJwtTokenHandler _JwtTokenHandler;

    public CreatePersonCommandHandler(IEkoDbContext dbContext, ILogger<CreatePersonCommandHandler> logger, IJwtTokenHandler JwtTokenHandler)
    {
        _dbContext = dbContext;
        _logger = logger;
        _JwtTokenHandler = JwtTokenHandler;
    }
    
    public async Task<string> Execute(CreatePersonCommand command)
    {
        if (await CheckPersonExists(command.email))
            throw new InvalidOperationException("User with this email already exists");

        var nameParts = command.fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var person = new Person
        {
            FirstName = nameParts[0],
            LastName = string.Join(" ", nameParts.Skip(1)),
            Email = command.email,
            Password = command.password
        };

        await CreatePerson(person);
        var jwt = await _JwtTokenHandler.GenerateToken(person);
        return jwt;
    }

    private async Task CreatePerson(Person person)
    {
        await _dbContext.Person.AddAsync(person);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<bool> CheckPersonExists(string email)
    {
        return await _dbContext.Person.AnyAsync(p => p.Email == email);
    }
}