using System.Windows.Input;
using Eko.Common.Cqrs;
using Eko.Database;
using Eko.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ICommand = Eko.Common.Cqrs.ICommand;

namespace Eko.Features;

public class PersonCommand : ICommand
{
    public string fullName {get; set;}
    public string email {get; set;}
    public string password {get; set;}
}

public class PersonCommandHandler : ICommandHandler<PersonCommand>
{
    private readonly IEkoDbContext _dbContext;
    private readonly ILogger _logger;

    public PersonCommandHandler(IEkoDbContext dbContext, ILogger<PersonCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task Execute(PersonCommand command)
    {
        _logger.LogInformation($"Executing command: {command}");
        _logger.LogInformation($"Full name: {command.fullName}");
        _logger.LogInformation($"Email: {command.email}");
        _logger.LogInformation($"Password: {command.password}");
        if (string.IsNullOrWhiteSpace(command.fullName))
            throw new ArgumentException("Full name cannot be empty");
            
        if (await CheckPersonExists(command.email))
            throw new InvalidOperationException("User with this email already exists");

        var nameParts = command.fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (nameParts.Length < 2)
            throw new ArgumentException("Full name must contain at least first and last name");

        var person = new Person
        {
            FirstName = nameParts[0],
            LastName = string.Join(" ", nameParts.Skip(1)),
            Email = command.email,
            Password = command.password // Рекомендуется хешировать пароль перед сохранением
        };

        await CreatePerson(person);
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