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
        public async Task<IActionResult> AddToWishlist(string courseId, string id)
        {
            var account = await _accountService.GetAccountById(id);

            var response = await _wishListService.AddWishList(account.Id, courseId);

            if (response.Status == "Error")
            {
                return Conflict(response);
            }

            return Ok(response);
        }

        [HttpDelete("DeleteWishListByCourseId")]
        public async Task<IActionResult> DeleteWishListByCourseId(string courseId, string accountId)
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
        public async Task<IActionResult> DeleteWishListByWishListId(string wishListId)
        {
            var response = await _wishListService.DeleteWishListByWishListId(wishListId);

            if (response.Status == "Error")
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("GetWishListByAccountId")]
        public async Task<IActionResult> GetWishListByAccountId(string id)
        {
            var account = await _accountService.GetAccountById(id);

            var response = await _wishListService.GetWishListByAccountId(account.Id);

            if (response == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
