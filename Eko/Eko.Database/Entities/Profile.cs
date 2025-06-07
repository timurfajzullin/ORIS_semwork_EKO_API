using System.ComponentModel.DataAnnotations;

namespace Eko.Database.Entities;

public class Profile
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string? Username { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string? DataOfBirth { get; set; }
    public string? Experience { get; set; }
    public string? Biography { get; set; }
    public string? Skills { get; set; }
    public string? Specialization { get; set; }
}