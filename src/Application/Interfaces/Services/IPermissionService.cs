using System;
using Application.Interfaces.Repositories;
using Domain.Entities.Authentication;

namespace Application.Interfaces.Repositories;

public interface IPermissionRepository : IGenericRepository<Permission>
{
    Task<List<string>> GetUserPermissionCodesAsync(int userId);
}
