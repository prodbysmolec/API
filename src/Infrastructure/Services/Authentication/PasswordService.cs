using System;
using Application.Authentication;
using Domain.Entities.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Authentication;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<User> _passwordHasher = new();
    public string HashPassword(string password)
    {
        var tempUser = new User
        {
            UserName = string.Empty,
            PasswordHash = string.Empty,
            Name = string.Empty,
            Nachname = string.Empty,
            Email = string.Empty
        }; // Temporärer User nur zum Hashen
        return _passwordHasher.HashPassword(tempUser, password);
    }

    public bool VerifyPassword(User user, string password)
    {
        return _passwordHasher.VerifyHashedPassword(
            user, 
            user.PasswordHash!, 
            password) != PasswordVerificationResult.Failed;
    }
}
