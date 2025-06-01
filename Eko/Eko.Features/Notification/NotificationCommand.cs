using Eko.Common.Cqrs;
using Eko.Common.EmailService;
using Eko.Database;

namespace Eko.Features.Notification;

public class NotificationCommand : ICommand
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
}

public class NotificationCommandHandler : ICommandHandler<NotificationCommand>
{
    private readonly IEkoDbContext _dbContext;
    private readonly IEmailService _emailService;

    public NotificationCommandHandler(IEkoDbContext dbContext, IEmailService emailService)
    {
        _dbContext = dbContext;
        _emailService = emailService;
    }
    
    public async Task Execute(NotificationCommand command)
    {
        await CreateNotification(command);
    }

    private async Task CreateNotification(NotificationCommand command)
    {
        var notification = new Database.Entities.Notification
        {
            Name = command.Name,
            Email = command.Email,
            Phone = command.Phone,
            Subject = command.Subject,
            Message = command.Message
        };
        _emailService.SendApplicationConfirmationAsync(notification);
        
        await _dbContext.Notification.AddAsync(notification);
        await _dbContext.SaveChangesAsync();
    }
}