using AutoMapper.Internal;
using Humanizer;
using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
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
        private readonly IEmailTemplateReader _emailTemplateReader;
        private readonly IMailService _mailService;

        public AccountController(IAccountService accountRepository, IEmailTemplateReader emailTemplateReader, IMailService mailService)
        {
            _accountService = accountRepository;
            _emailTemplateReader = emailTemplateReader;
            _mailService = mailService;
        }

        [HttpPost("SignUp")]
        public async Task<ActionResult> SignUp(SignUpModel signUpModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountService.SignUpAccountAsync(signUpModel);
                    if (result.Status.Equals("Success"))
                    {
                        var token = result.ConfirmEmailToken;
                        var url = Url.Action("ConfirmEmail", "Account", new {memberEmail = signUpModel.AccountEmail, tokenReset = token.Result}, Request.Scheme);
                        result.ConfirmEmailToken = null;

                        var body = await _emailTemplateReader.GetTemplate("Helper\\EmailTemplate.html");
                        body = string.Format(body, signUpModel.AccountEmail, url);

                        var messageRequest = new EmailRequest
                        {
                            To = signUpModel.AccountEmail,
                            Subject = "Confirm Email For Register",
                            Content = body
                        };

                        await _mailService.SendConFirmEmailAsync(messageRequest);

                        return Ok(result);
                    }
                    return BadRequest(result);
                }
                return ValidationProblem(ModelState);
            }
            catch
            {
                return BadRequest();
            }
        }
        
        [HttpPost("SignUpStaffAdmin")]
        public async Task<ActionResult> SignUpStaffAdminParent(SignUpModel signUpModel, RoleModel role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountService.SignUpAdminStaffAsync(signUpModel, role);
                    if (result.Status.Equals("Success"))
                    {
                        return Ok(result);
                    }
                    return BadRequest(result);
                }
                return ValidationProblem(ModelState);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult> SignIn(SignInModel signInModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountService.SignInAccountAsync(signInModel);
                    if (result.Status.Equals(false))
                    {
                        return Unauthorized(result);
                    }
                    return Ok(result);
                }
                return ValidationProblem(ModelState);
            }
            catch
            {
                return BadRequest();
            }

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

            var response = await _accountService.UpdateAccountProfile(updateProfileModel, accountId);

            if (response.Status == "Error")
            {
                return Conflict(response);
            }

            return Ok(response);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string tokenReset, string memberEmail)
        {
            try
            {
                var result = await _accountService.ConfirmEmail(memberEmail, tokenReset);
                if (result.Status.Equals(false))
                {
                    return Unauthorized(result);
                }
                return Redirect("https://online-class-room-fe.vercel.app/login");
            }
            catch
            {
                return BadRequest("Confirm email failed!");
            }
        }
        [HttpGet("ViewAccountList")]
        public async Task<ActionResult> ViewAccountList([FromQuery] AccountFilterParameters filterParams)
        {
            var account = await _accountService.ViewAccountList(filterParams);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }
        [HttpDelete("{accountId}")]
        public async Task<ActionResult> DeleteAccount(string accountId)
        {
            var result = await _accountService.DeleteAccount(accountId);
            if (result.Status.Equals("Success"))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
