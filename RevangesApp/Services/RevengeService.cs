using Microsoft.EntityFrameworkCore;
using RevengeApp.Data;
using RevengeApp.Models;

namespace RevengeApp.Services
{
    public class RevengeService : IRevengeService
    {
        private readonly RevengeContext _context;

        public RevengeService(RevengeContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Revenge>> GetAllRevengesForUserAsync(string userId)
        {
            return await _context.Revenges
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Date)
                .ToListAsync();
        }

        public async Task<Revenge?> GetRevengeByIdForUserAsync(int id, string userId)
        {
            return await _context.Revenges
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
        }

        public async Task<Revenge> AddRevengeForUserAsync(Revenge revenge, string userId)
        {
            revenge.UserId = userId;
            _context.Revenges.Add(revenge);
            await _context.SaveChangesAsync();
            return revenge;
        }

        public async Task<Revenge> UpdateRevengeForUserAsync(Revenge revenge, string userId)
        {
            var existingRevenge = await GetRevengeByIdForUserAsync(revenge.Id, userId);
            if (existingRevenge != null)
            {
                existingRevenge.Title = revenge.Title;
                existingRevenge.Description = revenge.Description;
                existingRevenge.Target = revenge.Target;
                existingRevenge.Date = revenge.Date;
                existingRevenge.Status = revenge.Status;
                existingRevenge.DramaLevel = revenge.DramaLevel;
                existingRevenge.Category = revenge.Category;
                existingRevenge.Notes = revenge.Notes;
                existingRevenge.CompletedDate = revenge.CompletedDate;

                await _context.SaveChangesAsync();
                return existingRevenge;
            }
            throw new InvalidOperationException("Revenge not found or access denied");
        }

        public async Task<bool> DeleteRevengeForUserAsync(int id, string userId)
        {
            var revenge = await GetRevengeByIdForUserAsync(id, userId);
            if (revenge != null)
            {
                _context.Revenges.Remove(revenge);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> CompleteRevengeForUserAsync(int id, string userId)
        {
            var revenge = await GetRevengeByIdForUserAsync(id, userId);
            if (revenge != null)
            {
                revenge.Status = RevengeStatus.Completed;
                revenge.CompletedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}