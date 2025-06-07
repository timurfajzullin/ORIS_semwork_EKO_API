using Eko.Common.Cqrs;
using Eko.Database;
using Eko.Features.Person;
using Microsoft.EntityFrameworkCore;

namespace Eko.Features.AdminPanel;

public class GetPersonsQuery : IQuery<List<Database.Entities.Person>>
{
    
}

public class GetPersonsQueryHandler : IQueryHandler<GetPersonsQuery, List<Database.Entities.Person>>
{
    private readonly IEkoDbContext _dbContext;

    public GetPersonsQueryHandler(IEkoDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<Database.Entities.Person>> Execute(GetPersonsQuery command)
    {
        return await GetPersons(command); 
    }

    private async Task<List<Database.Entities.Person>> GetPersons(GetPersonsQuery command)
    {
        return await _dbContext.Person.ToListAsync();
    }
}