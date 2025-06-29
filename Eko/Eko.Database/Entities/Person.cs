﻿namespace Eko.Database.Entities;

public class Person
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public bool IsAdmin { get; set; } = false;

    public int Plan { get; set; } = 1;
}