using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Eko.Common.Ai;
using Eko.Common.Cqrs;
using Eko.Database;
using Eko.Database.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Eko.Features.AskDeepSeek;

public class GetRequestAiQuery : IQuery<string>
{
    public string Query { get; set; }
    public string Type { get; set; }
}

public class GetRequestAiQueryHandler : IQueryHandler<GetRequestAiQuery, string>
{
    private readonly AiRequest _request;
    private readonly ILogger<GetRequestAiQueryHandler> _logger;
    private readonly IEkoDbContext _dbContext;

    public GetRequestAiQueryHandler(AiRequest request, ILogger<GetRequestAiQueryHandler> logger, IEkoDbContext dbContext)
    {
        _request = request;
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task<string> Execute(GetRequestAiQuery command)
    {
        _logger.LogInformation("GetRequestAiQueryHandler.Execute");
        var result = _request.GetResponse(command.Query);
        await AddMessage(command, await result);
        return await result;
    }

    private async Task AddMessage(GetRequestAiQuery command, string message)
    {
        _logger.LogInformation("GetRequestAiQueryHandler.AddMessage");

        var chat = new Chat
        {
            Type = command.Type,
            WhoseMessage = "Person",
            Message = command.Query
        };
        _dbContext.Chat.Add(chat);
        var aichat = new Chat
        {
            Type = command.Type,
            WhoseMessage = "Ai",
            Message = message
        };
        _dbContext.Chat.Add(aichat);
        await _dbContext.SaveChangesAsync();
    }
}