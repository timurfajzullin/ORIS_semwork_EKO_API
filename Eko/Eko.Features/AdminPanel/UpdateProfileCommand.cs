using Eko.Common.Cqrs;
using Eko.Database;
using Eko.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eko.Features.AdminPanel;

public class UpdateProfileCommand : ICommand
{
    public string Email { get; set; }
    public string BirthDate { get; set; }
    public string? Experience { get; set; }
    public string? Bio { get; set; }
    public string? Skills { get; set; }
    public string? Specialization { get; set; }
}

public class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand>
{
    private readonly IEkoDbContext _dbContext;

    public UpdateProfileCommandHandler(IEkoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Execute(UpdateProfileCommand command)
    {
        var profile = await _dbContext.Profile.FirstOrDefaultAsync(p => p.Email == command.Email);

        if (command.BirthDate != null) profile.DataOfBirth = command.BirthDate;
        if (command.Experience != null) profile.Experience = command.Experience;
        if (command.Bio != null) profile.Biography = command.Bio;
        if (command.Skills != null) profile.Skills = command.Skills;
        if (command.Specialization != null) profile.Specialization = command.Specialization;

        await _dbContext.SaveChangesAsync();
    }
}