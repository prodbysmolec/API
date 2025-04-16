using System;
using Domain.Entities.Authentication;

namespace Application.Authentication;

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(User user, string password);
}
