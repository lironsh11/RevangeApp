using RevengeApp.Models;

namespace RevengeApp.Services
{
    public interface IRevengeService
    {
        // User-specific methods
        Task<IEnumerable<Revenge>> GetAllRevengesForUserAsync(string userId);
        Task<Revenge?> GetRevengeByIdForUserAsync(int id, string userId);
        Task<Revenge> AddRevengeForUserAsync(Revenge revenge, string userId);
        Task<Revenge> UpdateRevengeForUserAsync(Revenge revenge, string userId);
        Task<bool> DeleteRevengeForUserAsync(int id, string userId);
        Task<bool> CompleteRevengeForUserAsync(int id, string userId);
    }
}