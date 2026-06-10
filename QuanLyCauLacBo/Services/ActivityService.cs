using Microsoft.EntityFrameworkCore;
using QuanLyCauLacBo.Data;
using QuanLyCauLacBo.Models.Domain;
using QuanLyCauLacBo.Services.Interfaces;

namespace QuanLyCauLacBo.Services
{
    public class ActivityService : IActivityService
    {
        private readonly ApplicationDbContext _context;

        public ActivityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Activity>> GetAllActivitiesAsync(string? search = null, string? status = null, int? clubId = null)
        {
            var query = _context.Activities
                .Include(a => a.Club)
                .Include(a => a.CreatedBy)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(a => a.Title.Contains(search) || (a.Description != null && a.Description.Contains(search)));

            if (!string.IsNullOrEmpty(status))
                query = query.Where(a => a.Status == status);

            if (clubId.HasValue)
                query = query.Where(a => a.ClubId == clubId.Value);

            return await query.OrderByDescending(a => a.StartDate).ToListAsync();
        }

        public async Task<Activity?> GetActivityByIdAsync(int id)
            => await _context.Activities
                .Include(a => a.Club)
                .Include(a => a.CreatedBy)
                .FirstOrDefaultAsync(a => a.Id == id);

        public async Task<Activity> CreateActivityAsync(Activity activity)
        {
            activity.CreatedAt = DateTime.Now;
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task<Activity> UpdateActivityAsync(Activity activity)
        {
            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task DeleteActivityAsync(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity != null)
            {
                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UpdateProgressAsync(int activityId, int progress, string? result = null)
        {
            var activity = await _context.Activities.FindAsync(activityId);
            if (activity == null) return false;

            activity.Progress = progress;
            if (!string.IsNullOrEmpty(result))
                activity.Result = result;

            if (progress >= 100)
                activity.Status = "Completed";
            else if (progress > 0)
                activity.Status = "InProgress";

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
