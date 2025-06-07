using Eko.Common.Cqrs;
using Eko.Database;
using Eko.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eko.Features.Person;

public class ChangePersonInfoCommand : ICommand
{
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string? DataOfBirth { get; set; }
    public string? Experience { get; set; }
    public string? Biography { get; set; }
    public string? Skills { get; set; }
    public string? Specialization { get; set; }
}

public class ChangePersonInfoCommandHandler : ICommandHandler<ChangePersonInfoCommand>
{
    private readonly IEkoDbContext _dbContext;
    private readonly ILogger<ChangePersonInfoCommandHandler> _logger;

    public ChangePersonInfoCommandHandler(IEkoDbContext dbContext, ILogger<ChangePersonInfoCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task Execute(ChangePersonInfoCommand command)
    {
        var profile = await _dbContext.Profile.FirstOrDefaultAsync(p => p.Email == command.Email);
        if (profile == null)
        {
            _logger.LogWarning("Profile not found with email" + command.Email);
            return;
        }
        
        profile.Phone = string.IsNullOrEmpty(command.Phone) ? profile.Phone : command.Phone;
        profile.DataOfBirth = string.IsNullOrEmpty(command.DataOfBirth) ? profile.DataOfBirth : command.DataOfBirth;
        profile.Experience = string.IsNullOrEmpty(command.Experience) ? profile.Experience : command.Experience;
        profile.Biography = string.IsNullOrEmpty(command.Biography) ? profile.Biography : command.Biography;
        profile.Skills = string.IsNullOrEmpty(command.Skills) ? profile.Skills : command.Skills;
        profile.Specialization = string.IsNullOrEmpty(command.Specialization) ? profile.Specialization : command.Specialization;

        await SavePersonInfo(profile);
    }

    private async Task SavePersonInfo(Profile profile)
    {
        _dbContext.Profile.Update(profile);
        _logger.LogInformation("Saved profile");
        await _dbContext.SaveChangesAsync();
    }
}