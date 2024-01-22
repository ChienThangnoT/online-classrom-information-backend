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
        //private readonly IAccountService _accountService;
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("SignUp")]
        public async Task<ActionResult> SignUp(SignUpModel signUpModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountRepository.SignUpAccountAsync(signUpModel);
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

        [HttpPost("SignIn")]
        public async Task<ActionResult> SignIn(SignInModel signInModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.SignInAccountAsync(signInModel);
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

                var result = await _accountRepository.RefreshToken(model);
                if (result.Status.Equals(false))
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }

        [HttpGet("{email}")]
        public async Task<ActionResult<AccountModel>> GetAccountById(string email)
        {
            var data = await _accountRepository.GetAccountByEmail(email);
            if (data != null)
            {
                return Ok(data);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
