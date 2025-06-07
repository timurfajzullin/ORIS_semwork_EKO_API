using Eko.Common.Cqrs;
using Eko.Database;
using Eko.Database.Entities;

namespace Eko.Features.AdminPanel;

public class RegisterPersonCommand : ICommand
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class RegisterPersonCommandHandler : ICommandHandler<RegisterPersonCommand>
{
    private readonly IEkoDbContext _dbContext;

    public RegisterPersonCommandHandler(IEkoDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Execute(RegisterPersonCommand command)
    {
        await RegisterPerson(command);
    }

    private async Task RegisterPerson(RegisterPersonCommand command)
    {
        var person = new Database.Entities.Person
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            Password = command.Password,
            IsAdmin = command.IsAdmin
        };
        var profile = new Profile
        {
            Id = person.Id,
            Name = command.FirstName + " " + command.LastName,
            Email = command.Email,
        };
        await _dbContext.Person.AddAsync(person);
        await _dbContext.Profile.AddAsync(profile);
        await _dbContext.SaveChangesAsync();
    }
}