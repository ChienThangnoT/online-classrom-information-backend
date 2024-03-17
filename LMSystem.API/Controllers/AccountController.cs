using AutoMapper.Internal;
using Humanizer;
using LMSystem.API.Helper;
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
        private readonly INotificationService _notificationService;

        public AccountController(IAccountService accountRepository, IEmailTemplateReader emailTemplateReader, IMailService mailService, INotificationService notificationService)
        {
            _accountService = accountRepository;
            _emailTemplateReader = emailTemplateReader;
            _mailService = mailService;
            _notificationService = notificationService;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountService.SignUpAccountAsync(signUpModel);
                    var parentResult = await _accountService.SignUpParentAsync(signUpModel.ParentEmail);

                    if (result.Status.Equals("Success") && parentResult.Status.Equals("Success"))
                    {
                        // account student
                        var token = result.ConfirmEmailToken;
                        var url = Url.Action("ConfirmEmail", "Account", new { memberEmail = signUpModel.AccountEmail, tokenReset = token.Result }, Request.Scheme);
                        result.ConfirmEmailToken = null;

                        //var body = await _emailTemplateReader.GetTemplate("Helper\\EmailTemplate.html");
                        //body = string.Format(body, signUpModel.AccountEmail, url);

                        var messageRequest = new EmailRequest
                        {
                            To = signUpModel.AccountEmail,
                            Subject = "Xác thực email đã đăng ký",
                            Content = MailTemplate.ConfirmTemplate(signUpModel.AccountEmail, url)
                        };

                        await _mailService.SendConFirmEmailAsync(messageRequest);

                        //account parent
                        var parenToken = parentResult.ConfirmEmailToken;
                        var parentUrl = Url.Action("ConfirmParentEmail", "Account", new { memberEmail = signUpModel.ParentEmail, tokenReset = parenToken.Result }, Request.Scheme);
                        parentResult.ConfirmEmailToken = null;

                        //var body = await _emailTemplateReader.GetTemplate("Helper\\EmailTemplate.html");
                        //body = string.Format(body, signUpModel.AccountEmail, url);

                        var messageParentRequest = new EmailRequest
                        {
                            To = signUpModel.ParentEmail,
                            Subject = "Xác thực email phụ huynh mà học sinh tên " + signUpModel.FirstName + " "+ signUpModel.LastName +" đã đăng ký",
                            Content = ParentConfirmEmailTemplate.ConfirmTemplate(signUpModel.ParentEmail, parentUrl, parentResult.DataObject.ToString())
                        };

                        
                        await _mailService.SendConFirmEmailAsync(messageParentRequest);

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
        public async Task<IActionResult> SignUpStaffAdminParent(SignUpModel signUpModel, RoleModel role)
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
        public async Task<IActionResult> SignIn(SignInModel signInModel)
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
        public async Task<IActionResult> GetAccountByEmail(string email)
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
        [Authorize]
        public async Task<IActionResult> UpdateProfile(UpdateProfileModel updateProfileModel, string accountId)
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
                var account = await _accountService.GetAccountByEmail(memberEmail);
                if (account != null)
                {
                    Notification notification = new Notification
                    {
                        AccountId = account.Id,
                        SendDate = DateTime.Now,
                        Type = NotificationType.System.ToString(),
                        Title = "Chào mừng bạn đến với eStudyHub",
                        Message = "Cảm ơn bạn đã chọn eStudyHub để học tập. Hãy cùng nhau trải nghiệm các khóa học nhé!"
                    };
                    await _notificationService.AddNotificationByAccountId(account.Id, notification);
                    return Redirect("https://online-class-room-fe.vercel.app/login");
                }
                return Ok(result);
            }
            catch
            {
                return BadRequest("Confirm email failed!");
            }
        }


        [HttpGet("ConfirmParentEmail")]
        public async Task<IActionResult> ConfirmParentEmail(string tokenReset, string memberEmail)
        {
            try
            {
                var result = await _accountService.ConfirmEmail(memberEmail, tokenReset);
                if (result.Status.Equals(false))
                {
                    return Unauthorized(result);
                }
                var account = await _accountService.GetAccountByEmail(memberEmail);
                if (account != null)
                {
                    Notification notification = new Notification
                    {
                        AccountId = account.Id,
                        SendDate = DateTime.Now,
                        Type = NotificationType.System.ToString(),
                        Title = "Chào mừng phụ huynh đến với eStudyHub",
                        Message = "Cảm ơn bạn đã tin tưởng lựa chọn eStudyHub để cho con mình học tập. Hãy cùng nhau trải nghiệm các khóa học bổ ích!"
                    };
                    await _notificationService.AddNotificationByAccountId(account.Id, notification);
                    return Redirect("https://online-class-room-fe.vercel.app/login");
                }
                return Ok(result);
            }
            catch
            {
                return BadRequest("Confirm email failed!");
            }
        }

        [HttpGet("ViewAccountList")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ViewAccountList([FromQuery] AccountFilterParameters filterParams)
        {
            var account = await _accountService.ViewAccountList(filterParams);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpGet("GetAccountByParentAccountId")]
        [Authorize]
        public async Task<IActionResult> GetAccountByParentAccountId(string accountId)
        {
            var account = await _accountService.GetAccountByParentAccountId(accountId);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpDelete("{accountId}")]
        public async Task<IActionResult> DeleteAccount(string accountId)
        {
            var result = await _accountService.DeleteAccount(accountId);
            if (result.Status.Equals("Success"))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("Send-email")]
        public async Task<IActionResult> SendEmail([FromForm] EmailRequest email)
        {
            try
            {
                await _mailService.SendEmailAsync(email);
                return Ok("Test");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update-device-token")]
        public async Task<IActionResult> UpdateDeviceToken(DeviceTokenModal deviceToken)
        {
            try
            {
                var result = await _accountService.UpdateDeviceToken(deviceToken.AccountId, deviceToken.DeviceToken);
                if (result)
                {
                    return Ok(new ResponeModel
                    {
                        Status = "Success",
                        Message = "Update token successfully"
                    });
                }
                return BadRequest(new ResponeModel
                {
                    Status = "Error",
                    Message = "Can not update token"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("CountTotalStudent")]
        public async Task<IActionResult> CountTotalStudent()
        {
            var response = await _accountService.CountTotalStudent();
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpGet("CountTotalAccount")]
        public async Task<IActionResult> CountTotalAccount()
        {
            var response = await _accountService.CountTotalAccount();
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
