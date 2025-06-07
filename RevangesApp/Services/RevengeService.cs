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

        public async Task<IEnumerable<Revenge>> GetAllRevengesAsync()
        {
            return await _context.Revenges.OrderByDescending(r => r.Date).ToListAsync();
        }

        public async Task<Revenge?> GetRevengeByIdAsync(int id)
        {
            return await _context.Revenges.FindAsync(id);
        }

        public async Task<Revenge> AddRevengeAsync(Revenge revenge)
        {
            _context.Revenges.Add(revenge);
            await _context.SaveChangesAsync();
            return revenge;
        }

        public async Task<Revenge> UpdateRevengeAsync(Revenge revenge)
        {
            _context.Entry(revenge).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return revenge;
        }

        public async Task<bool> DeleteRevengeAsync(int id)
        {
            var revenge = await _context.Revenges.FindAsync(id);
            if (revenge != null)
            {
                _context.Revenges.Remove(revenge);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Revenge>> SearchRevengesAsync(string searchTerm)
        {
            return await _context.Revenges
                .Where(r => r.Title.Contains(searchTerm) ||
                           r.Description.Contains(searchTerm) ||
                           r.Target.Contains(searchTerm))
                .OrderByDescending(r => r.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Revenge>> FilterRevengesAsync(RevengeStatus? status, RevengeCategory? category, int? dramaLevel)
        {
            var query = _context.Revenges.AsQueryable();

            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);

            if (category.HasValue)
                query = query.Where(r => r.Category == category.Value);

            if (dramaLevel.HasValue)
                query = query.Where(r => r.DramaLevel == dramaLevel.Value);

            return await query.OrderByDescending(r => r.Date).ToListAsync();
        }

        public async Task<bool> CompleteRevengeAsync(int id)
        {
            var revenge = await _context.Revenges.FindAsync(id);
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