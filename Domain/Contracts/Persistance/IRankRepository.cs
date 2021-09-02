using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Contracts.Persistance
{
    public interface IRankRepository
    {
        Task AddRankAsync(ulong id, ulong roleId);
        Task ClearRanksAsync(List<Rank> ranks);
        Task<List<Rank>> GetRanksAsync(ulong id);
        Task RemoveRankAsync(ulong id, ulong roleId);
    }
}