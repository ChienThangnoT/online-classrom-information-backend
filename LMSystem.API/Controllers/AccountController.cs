using LMSystem.Repository.Data;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Repositories;
using LMSystem.Services.Interfaces;
using LMSystem.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        //private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountService accountRepository)
        {
            _accountService = accountRepository;
        }

        [HttpPost("SignUp")]
        public async Task<ActionResult> SignUp(SignUpModel signUpModel)
        {
            //try
            //{
            if (ModelState.IsValid)
            {
                var result = await _accountService.SignUpAccountAsync(signUpModel);
                if (result.Status.Equals("Success"))
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return ValidationProblem(ModelState);
            //}
            //catch { return BadRequest(); }
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult> SignIn(SignInModel signInModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.SignInAccountAsync(signInModel);
                if (result.Status.Equals(false))
                {
                    return Unauthorized();
                }
                return Ok(result);
            }
            return ValidationProblem(ModelState);
        }


        [HttpPost("Refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel model)
        {

            var result = await _accountService.RefreshToken(model);
            if (result.Status.Equals(false))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountService.ChangePasswordAsync(model);
                    if (result.Status.Equals("Success"))
                    {
                        return Ok(result);
                    }
                    return BadRequest(result);
                }
                return ValidationProblem(ModelState);
            }
            catch { return BadRequest(); }
        }

        [HttpGet("{email}")]
        [Authorize]
        public async Task<ActionResult<AccountModel>> GetAccountByEmail(string email)
        {
            var data = await _accountService.GetAccountByEmail(email);
            if (data != null)
            {
                return Ok(data);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut("UpdateProfile")]
        public async Task<ActionResult> UpdateProfile(UpdateProfileModel updateProfileModel, string accountId)
        {
            var account = await _accountService.GetAccountById(accountId);

            var response = await _accountService.UpdateAccountProfile(updateProfileModel,accountId);

            if (response.Status == "Error")
            {
                return Conflict(response);
            }

            return Ok(response);
        }
    }
}
