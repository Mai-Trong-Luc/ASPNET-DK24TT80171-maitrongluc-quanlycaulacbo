using QuanLyCauLacBo.Models.Domain;

namespace QuanLyCauLacBo.Services.Interfaces
{
    public interface IClubService
    {
        Task<List<Club>> GetAllClubsAsync(string? search = null, string? status = null);
        Task<Club?> GetClubByIdAsync(int id);
        Task<Club> CreateClubAsync(Club club);
        Task<Club> UpdateClubAsync(Club club);
        Task DeleteClubAsync(int id);
        Task<bool> JoinClubAsync(int clubId, int userId);
        Task<bool> LeaveClubAsync(int clubId, int userId);
        Task<bool> ApproveMemberAsync(int clubMemberId);
        Task<bool> RejectMemberAsync(int clubMemberId);
        Task<ClubMember?> GetMembershipAsync(int clubId, int userId);
        Task<List<ClubMember>> GetClubMembersAsync(int clubId);
        Task<bool> UpdateMemberRoleAsync(int clubMemberId, string role);
    }
}
