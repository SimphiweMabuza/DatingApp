using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Extentions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class ViewsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public ViewsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddView(string username)
        {
            var sourceUserId = User.GetUserId();
            var ViewedUser =  await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _unitOfWork.ViewsRepository.GetUserWithViews(sourceUserId);

            if (ViewedUser == null) return NotFound();

            var userView = await _unitOfWork.ViewsRepository.GetUserView(sourceUserId, ViewedUser.Id);

            userView = new UserView
            {
                SourceUserId = sourceUserId,
                ViewedUserId = ViewedUser.Id
            };

            sourceUser.ViewedUsers.Add(userView);

           if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to View user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ViewDto>>> GetUserViews([FromQuery]ViewsParams viewsParams)
        {
            viewsParams.UserId = User.GetUserId();
            var users = await _unitOfWork.ViewsRepository.GetUserViews(viewsParams);

            Response.AddPaginationHeader(users.CurrentPage, 
                users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }
    }
}