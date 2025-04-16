using System;
using Application.Interfaces.Repositories;
using Domain.Entities.Authentication;

namespace Application.Interfaces.Services;

public interface IPermissionService : IGenericRepository<Permission>
{
    Task<List<string>> GetUserPermissionCodesAsync(int userId);
}
