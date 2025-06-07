using System.Security.Claims;
using Eko.Common.Cqrs;
using Eko.Database;
using Eko.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eko.Features.Person;

public class GetPersonInfoQuery : IQuery<Profile>
{
    public string Email { get; set; }
}

public class GetPersonInfoQueryHandler : IQueryHandler<GetPersonInfoQuery, Profile>
{
    private readonly IEkoDbContext _dbContext;
    private readonly ILogger<GetPersonInfoQueryHandler> _logger;
    
    public GetPersonInfoQueryHandler(IEkoDbContext dbContext, ILogger<GetPersonInfoQueryHandler> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<Profile> Execute(GetPersonInfoQuery command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        return await GetPerson(command.Email);
    }
    
    private async Task<Profile> GetPerson(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or whitespace", nameof(email));
        }

        return await _dbContext.Profile.FirstOrDefaultAsync(p => p.Email == email);
    }
}