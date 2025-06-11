using Eko.Common.Cqrs;
using Eko.Database;
using Eko.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eko.Features.AskDeepSeek;

public class GetChatsItemsQuery : IQuery<List<Chat>>
{
    
}

public class GetChatsItemsQueryHandler : IQueryHandler<GetChatsItemsQuery, List<Chat>>
{
    private readonly IEkoDbContext _dbContext;
    private readonly ILogger<GetChatsItemsQueryHandler> _logger;

    public GetChatsItemsQueryHandler(IEkoDbContext dbContext, ILogger<GetChatsItemsQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<List<Chat>> Execute(GetChatsItemsQuery command)
    {
        return await _dbContext.Chat.ToListAsync();
    }
}