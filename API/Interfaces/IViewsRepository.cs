using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IViewsRepository
    {
        Task<UserView> GetUserView(int sourceUserId, int ViewedUserId);
        Task<AppUser> GetUserWithViews(int userId);
        Task<PagedList<ViewDto>> GetUserViews(ViewsParams viewsParams);
    }
}