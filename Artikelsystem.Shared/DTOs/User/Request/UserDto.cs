using System;

namespace Artikelsystem.Shared.DTOs.User.Request;

public class UserDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public string? Name { get; set; }
    public string? Nachname { get; set; }
    public string? Email { get; set; }
}
