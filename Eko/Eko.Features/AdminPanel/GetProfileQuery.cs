using Eko.Common.Cqrs;
using Eko.Database;
using Eko.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eko.Features.AdminPanel;

public class GetProfileQuery : IQuery<Profile>
{
    public string Email { get; set; }
}

public class GetProfileQueryHandler : IQueryHandler<GetProfileQuery, Profile>
{
    private readonly EkoDbContext _dbContext;

    public GetProfileQueryHandler(EkoDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Profile> Execute(GetProfileQuery command)
    {
        return await GetProfile(command);
    }

    private async Task<Profile> GetProfile(GetProfileQuery command)
    {
        return await _dbContext.Profile.FirstOrDefaultAsync(x => x.Email == command.Email);
    }
}