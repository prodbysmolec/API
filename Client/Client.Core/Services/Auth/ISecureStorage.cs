using System;

namespace Client.Core.Services.Auth;

public interface ISecureStorage
{
    Task<string> GetAsync(string key);
    Task SetAsync(string key, string value);
    Task RemoveAsync(string key);
    Task<bool> ContainsKeyAsync(string key);
}
