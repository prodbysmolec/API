using Artikelsystem.Api.Features.Lieferant.Models.Entitys;
using Artikelsystem.Api.Infrastructure.Persistence.Repositories;

namespace Artikelsystem.Api.Features.Lieferant.Repositories;

public interface ILieferantRepository : IRepository<Models.Entitys.Lieferant>
{
    Task<Models.Entitys.Lieferant?> GetLieferantMitWareneingaengenAsync(int id);
}