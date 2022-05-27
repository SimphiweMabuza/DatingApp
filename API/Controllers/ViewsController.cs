using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class ViewsController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IViewsRepository _ViewsRepository;
        public ViewsController(IUserRepository userRepository, IViewsRepository ViewsRepository)
        {
            _ViewsRepository = ViewsRepository;
            _userRepository = userRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();
            var ViewdUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _ViewsRepository.GetUserWithLikes(sourceUserId);
            
            if (ViewdUser == null) return NotFound();

            if (sourceUser.UserName == username) return BadRequest("You cannot View yourself");

            var userView = await _ViewsRepository.GetUserLike(sourceUserId, ViewdUser.Id);

            if (userView != null) return BadRequest("You already Viewd this user");

            userView = new UserView
            {
                SourceUserId = sourceUserId,
                ViewdUserId = ViewdUser.Id
            };

            sourceUser.LikedUsers.Add(userView);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ViewDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await _ViewsRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(users.CurrentPage, 
                users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }
    }
}