using RevengeApp.Models;

namespace RevengeApp.Services
{
    public interface IRevengeService
    {
        Task<IEnumerable<Revenge>> GetAllRevengesAsync();
        Task<Revenge?> GetRevengeByIdAsync(int id);
        Task<Revenge> AddRevengeAsync(Revenge revenge);
        Task<Revenge> UpdateRevengeAsync(Revenge revenge);
        Task<bool> DeleteRevengeAsync(int id);
        Task<IEnumerable<Revenge>> SearchRevengesAsync(string searchTerm);
        Task<IEnumerable<Revenge>> FilterRevengesAsync(RevengeStatus? status, RevengeCategory? category, int? dramaLevel);
        Task<bool> CompleteRevengeAsync(int id);
    }
}