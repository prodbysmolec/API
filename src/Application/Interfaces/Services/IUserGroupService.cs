using System;

namespace Application.Interfaces.Services;

public interface IUserGruppenService
{
    Task<IEnumerable<object>> GetUserGroupNamesAsync(int id);
}
