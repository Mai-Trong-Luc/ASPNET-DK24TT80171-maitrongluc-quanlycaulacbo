using Microsoft.EntityFrameworkCore;
using QuanLyCauLacBo.Data;
using QuanLyCauLacBo.Models.Domain;
using QuanLyCauLacBo.Services.Interfaces;

namespace QuanLyCauLacBo.Services
{
    public class ClubService : IClubService
    {
        private readonly ApplicationDbContext _context;

        public ClubService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Club>> GetAllClubsAsync(string? search = null, string? status = null)
        {
            var query = _context.Clubs
                .Include(c => c.Members)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(c => c.Name.Contains(search) || (c.Description != null && c.Description.Contains(search)));

            if (!string.IsNullOrEmpty(status))
                query = query.Where(c => c.Status == status);

            return await query.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Club?> GetClubByIdAsync(int id)
            => await _context.Clubs
                .Include(c => c.Members).ThenInclude(m => m.User)
                .Include(c => c.Events)
                .Include(c => c.Activities)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Club> CreateClubAsync(Club club)
        {
            club.CreatedAt = DateTime.Now;
            _context.Clubs.Add(club);
            await _context.SaveChangesAsync();
            return club;
        }

        public async Task<Club> UpdateClubAsync(Club club)
        {
            _context.Clubs.Update(club);
            await _context.SaveChangesAsync();
            return club;
        }

        public async Task DeleteClubAsync(int id)
        {
            var club = await _context.Clubs.FindAsync(id);
            if (club != null)
            {
                _context.Clubs.Remove(club);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> JoinClubAsync(int clubId, int userId)
        {
            var existing = await _context.ClubMembers
                .FirstOrDefaultAsync(cm => cm.ClubId == clubId && cm.UserId == userId);

            if (existing != null) return false;

            _context.ClubMembers.Add(new ClubMember
            {
                ClubId = clubId,
                UserId = userId,
                Role = "Member",
                Status = "Pending",
                JoinedDate = DateTime.Now
            });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LeaveClubAsync(int clubId, int userId)
        {
            var member = await _context.ClubMembers
                .FirstOrDefaultAsync(cm => cm.ClubId == clubId && cm.UserId == userId);

            if (member == null) return false;

            _context.ClubMembers.Remove(member);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveMemberAsync(int clubMemberId)
        {
            var member = await _context.ClubMembers.FindAsync(clubMemberId);
            if (member == null) return false;

            member.Status = "Active";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectMemberAsync(int clubMemberId)
        {
            var member = await _context.ClubMembers.FindAsync(clubMemberId);
            if (member == null) return false;

            member.Status = "Rejected";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ClubMember?> GetMembershipAsync(int clubId, int userId)
            => await _context.ClubMembers
                .FirstOrDefaultAsync(cm => cm.ClubId == clubId && cm.UserId == userId);

        public async Task<List<ClubMember>> GetClubMembersAsync(int clubId)
            => await _context.ClubMembers
                .Include(cm => cm.User)
                .Where(cm => cm.ClubId == clubId)
                .OrderBy(cm => cm.Status)
                .ThenBy(cm => cm.User.FullName)
                .ToListAsync();

        public async Task<bool> UpdateMemberRoleAsync(int clubMemberId, string role)
        {
            var member = await _context.ClubMembers.FindAsync(clubMemberId);
            if (member == null) return false;
            member.Role = role;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
