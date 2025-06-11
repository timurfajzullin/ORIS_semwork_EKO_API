namespace Eko.Database.Entities;

public class Chat
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Type { get; set; }
    public string WhoseMessage { get; set; }
    public string Message { get; set; }
}