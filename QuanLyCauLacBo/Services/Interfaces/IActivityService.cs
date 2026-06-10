using QuanLyCauLacBo.Models.Domain;

namespace QuanLyCauLacBo.Services.Interfaces
{
    public interface IActivityService
    {
        Task<List<Activity>> GetAllActivitiesAsync(string? search = null, string? status = null, int? clubId = null);
        Task<Activity?> GetActivityByIdAsync(int id);
        Task<Activity> CreateActivityAsync(Activity activity);
        Task<Activity> UpdateActivityAsync(Activity activity);
        Task DeleteActivityAsync(int id);
        Task<bool> UpdateProgressAsync(int activityId, int progress, string? result = null);
    }
}
