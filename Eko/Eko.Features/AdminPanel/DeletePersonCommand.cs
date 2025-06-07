using Eko.Common.Cqrs;
using Eko.Database;
using Microsoft.EntityFrameworkCore;

namespace Eko.Features.AdminPanel;

public class DeletePersonCommand : ICommand
{
    public string Email { get; set; }
}

public class DeletePersonCommandHandler : ICommandHandler<DeletePersonCommand>
{
    private readonly EkoDbContext _dbContext;

    public DeletePersonCommandHandler(EkoDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Execute(DeletePersonCommand command)
    {
        await DeletePerson(command);
    }

    private async Task DeletePerson(DeletePersonCommand command)
    {
        var person = await _dbContext.Person.FirstOrDefaultAsync(p => p.Email == command.Email);
        var profile = await _dbContext.Profile.FirstOrDefaultAsync(p => p.Email == command.Email);
        _dbContext.Profile.Remove(profile);
        _dbContext.Person.Remove(person);
        await _dbContext.SaveChangesAsync();
    }
}