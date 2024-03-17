using LMSystem.Repository.Models;
using LMSystem.Services.Interfaces;
using LMSystem.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;

        private readonly IAccountService _accountService;


        public WishListController(IWishListService wishListService, IAccountService accountService)
        {
            _wishListService = wishListService;
            _accountService = accountService;
        }

        [HttpPost("AddWishList")]
        [Authorize]
        public async Task<IActionResult> AddToWishlist(int courseId, string accountId)
        {
            var account = await _accountService.GetAccountById(accountId);

            var response = await _wishListService.AddWishList(account.Id, courseId);

            if (response.Status == "Error")
            {
                return Conflict(response);
            }

            return Ok(response);
        }

        [HttpDelete("DeleteWishListByCourseId")]
        [Authorize]
        public async Task<IActionResult> DeleteWishListByCourseId(int courseId, string accountId)
        {
            var account = await _accountService.GetAccountById(accountId);

            var response = await _wishListService.DeleteWishListByCourseId(courseId, account.Id);

            if (response.Status == "Error")
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("DeleteWishListByWishListId")]
        [Authorize]
        public async Task<IActionResult> DeleteWishListByWishListId(int wishListId)
        {
            var response = await _wishListService.DeleteWishListByWishListId(wishListId);

            if (response.Status == "Error")
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("GetWishListByAccountId")]
        [Authorize]
        public async Task<IActionResult> GetWishListByAccountId(string accountId)
        {
            var account = await _accountService.GetAccountById(accountId);

            var response = await _wishListService.GetWishListByAccountId(account.Id);

            if (response == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        [HttpGet("CountTotalWishListByAccountId")]
        public async Task<IActionResult> CountTotalWishListByAccountId(string accountId)
        {
            var response = await _wishListService.CountTotalWishListByAccountId(accountId);

            if (response.Status == "Error")
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
