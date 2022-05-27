using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ViewsRepository : IViewsRepository
    {
        private readonly DataContext _context;
        public ViewsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserView> GetUserView(int sourceUserId, int VieweddUserId)
        {
            return await _context.Views.FindAsync(sourceUserId, VieweddUserId);
        }

        public async Task<PagedList<ViewDto>> GetUserViews(ViewsParams ViewsParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var Views = _context.Views.AsQueryable();

            if (ViewsParams.Predicate == "Viewed")
            {
                Views = Views.Where(View => View.SourceUserId == ViewsParams.UserId);
                users = Views.Select(View => View.ViewedUser);
            }

            if (ViewsParams.Predicate == "ViewedBy")
            {
                Views = Views.Where(View => View.ViewedUserId == ViewsParams.UserId);
                users = Views.Select(View => View.SourceUser);
            }

            var ViewedUsers = users.Select(user => new ViewDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id
            });

            return await PagedList<ViewDto>.CreateAsync(ViewedUsers, 
                ViewsParams.PageNumber, ViewsParams.PageSize);
        }

        public async Task<AppUser> GetUserWithViews(int userId)
        {
            return await _context.Users
                .Include(x => x.ViewedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}