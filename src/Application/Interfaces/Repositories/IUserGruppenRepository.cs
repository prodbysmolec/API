using System;

namespace Application.Interfaces.Repositories;

public interface IUserGruppenRepository
{
    Task<IEnumerable<object>> GetUserGroupNamesAsync(int id);
}
