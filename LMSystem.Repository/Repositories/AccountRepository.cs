using AutoMapper;
using LMSystem.Repository.Data;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly LMOnlineSystemDbContext _context;
        private readonly IConfiguration configuration;
        private readonly SignInManager<Account> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<Account> userManager;
        private readonly IMapper _mapper;

        public AccountRepository(
            LMOnlineSystemDbContext context,
            IConfiguration configuration,
            UserManager<Account> userManager,
            SignInManager<Account> signInManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            this._context = context;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this._mapper = mapper;
        }

        public async Task<AccountModel> GetAccountByEmail(string email)
        {
            var account = await userManager.FindByNameAsync(email);
            var result = _mapper.Map<AccountModel>(account);
            return result;
        }

        public async Task<Account> GetAccountById(string id)
        {
            var account = await userManager.FindByIdAsync(id);
            return account;
        }

        public async Task<AuthenticationResponseModel> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return new AuthenticationResponseModel
                {
                    Status = false,
                    Message = "Request not valid!"
                };
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return new AuthenticationResponseModel
                {
                    Status = false,
                    Message = "Invalid access token or refresh token!"
                };
            }

            string username = principal.Identity.Name;

            var user = await userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return new AuthenticationResponseModel
                {
                    Status = false,
                    Message = "Invalid access token or refresh token!"
                };
            }

            var newAccessToken = CreateToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await userManager.UpdateAsync(user);

            return new AuthenticationResponseModel
            {
                Status = true,
                Message = "Refresh Token successfully!",
                JwtToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                Expired = newAccessToken.ValidTo,
                JwtRefreshToken = newRefreshToken
            };
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
            _ = int.TryParse(configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Token unavailabel!");
            return principal;
        }

        public async Task<AuthenticationResponseModel> SignInAccountAsync(SignInModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.AccountEmail, model.AccountPassword, false, false);
            var account = await userManager.FindByNameAsync(model.AccountEmail);

            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(model.AccountEmail);
                //var validPass = await userManager.CheckPasswordAsync(user, model.AccountPassword);
                if (user != null /*|| validPass*/)
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.AccountEmail),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var userRole = await userManager.GetRolesAsync(user);
                    foreach (var role in userRole)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                    }

                    var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));

                    var token = new JwtSecurityToken(
                        issuer: configuration["JWT:ValidIssuer"],
                        audience: configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(2),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256)
                    );

                    var refreshToken = GenerateRefreshToken();

                    _ = int.TryParse(configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                    account.RefreshToken = refreshToken;
                    account.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                    await userManager.UpdateAsync(account);

                    return new AuthenticationResponseModel
                    {
                        Status = true,
                        Message = "Login successfully!",
                        JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
                        Expired = token.ValidTo,
                        JwtRefreshToken = refreshToken,
                    };
                }
                else
                {
                    return new AuthenticationResponseModel { Status = false, Message = "Cannot find user" };
                }
            }
            else if (result.IsNotAllowed)
            {
                var token = userManager.GenerateEmailConfirmationTokenAsync(account);
                return new AuthenticationResponseModel
                {
                    Status = false,
                    Message = "Email xác nhận đã được gửi đến tài khoản email bạn đã đăng ký, vui lòng xác thực tài khoản để đăng nhập!",
                    VerifyEmailToken = token
                };
            }
            else
            {
                return new AuthenticationResponseModel { Status = false, Message = "Sai tài khoản hoặc mật khẩu!" };
            }
        }

        public async Task<ResponeModel> SignUpAccountAsync(SignUpModel model)
        {
            try
            {
                var exsistAccount = await userManager.FindByNameAsync(model.AccountEmail);
                if (exsistAccount == null)
                {
                    var user = new Account
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        BirthDate = model.BirthDate,
                        Status = "Is Active",
                        UserName = model.AccountEmail,

                        Email = model.AccountEmail,
                        PhoneNumber = model.AccountPhone
                    };
                    var result = await userManager.CreateAsync(user, model.AccountPassword);
                    string errorMessage = null;
                    if (result.Succeeded)
                    {
                        if (!await roleManager.RoleExistsAsync(RoleModel.Student.ToString()))
                        {
                            await roleManager.CreateAsync(new IdentityRole(RoleModel.Student.ToString()));
                        }
                        if (await roleManager.RoleExistsAsync(RoleModel.Student.ToString()))
                        {
                            await userManager.AddToRoleAsync(user, RoleModel.Student.ToString());
                        }
                        var token = userManager.GenerateEmailConfirmationTokenAsync(user);
                        //token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));


                        return new ResponeModel { Status = "Success", Message = "Create account successfull, Please confirm your email to login into eHubSystem", ConfirmEmailToken = token };
                    }
                    foreach (var ex in result.Errors)
                    {
                        errorMessage = ex.Description;
                    }
                    return new ResponeModel { Status = "Error", Message = errorMessage };
                }
                return new ResponeModel { Status = "Error", Message = "Account already exist" };
            }
            catch (Exception ex)
            {
                // Log or print the exception details
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while checking if the account exists." };
            }

        }

        public Task<AccountModel> UpdateAccountByEmail(AccountModel account)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponeModel> UpdateAccountProfile(UpdateProfileModel updateProfileModel, string accountId)
        {
            try
            {
                var existingAccount = await _context.Account.FirstOrDefaultAsync(a => a.Id == accountId);

                if (existingAccount == null)
                {
                    return new ResponeModel { Status = "Error", Message = "Account not found" };
                }

                existingAccount = submitAccountChanges(existingAccount, updateProfileModel);

                await _context.SaveChangesAsync();

                return new ResponeModel { Status = "Success", Message = "Account profile updated successfully", DataObject = existingAccount };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while updating the account profile" };
            }
        }

        private Account submitAccountChanges(Account account, UpdateProfileModel updateProfileModel)
        {
            account.FirstName = updateProfileModel.FirstName;
            account.LastName = updateProfileModel.LastName;
            account.Email = updateProfileModel.Email;
            account.PhoneNumber = updateProfileModel.PhoneNumber;
            account.BirthDate = updateProfileModel.BirthDate;
            account.Biography = updateProfileModel.Biography;
            account.ProfileImg = updateProfileModel.ProfileImg;
            account.Sex = updateProfileModel.Sex;
            return account;
        }
        public async Task<ResponeModel> ChangePasswordAsync(ChangePasswordModel model)
        {
            var account = await userManager.FindByEmailAsync(model.Email);
            if (account == null)
            {
                return new ResponeModel
                {
                    Status = "Error",
                    Message = "Can not find your account!"
                };
            }
            var result = await userManager.ChangePasswordAsync(account, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return new ResponeModel
                {
                    Status = "Error",
                    Message = "Cannot change pass"
                };
            }

            return new ResponeModel
            {
                Status = "Success",
                Message = "Change password successfully!"
            };
        }

        public async Task<ResponeModel> SignUpAdminStaffAsync(SignUpModel model, RoleModel role)
        {
            try
            {
                var exsistAccount = await userManager.FindByNameAsync(model.AccountEmail);
                if (exsistAccount == null)
                {
                    var user = new Account
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        BirthDate = model.BirthDate,
                        Status = "Is Active",
                        UserName = model.AccountEmail,

                        Email = model.AccountEmail,
                        PhoneNumber = model.AccountPhone
                    };

                    string errorMessage = null;

                    if (role.Equals(RoleModel.Admin) || role.Equals(RoleModel.Staff))
                    {
                        await userManager.CreateAsync(user, model.AccountPassword);

                        if (!await roleManager.RoleExistsAsync(role.ToString()))
                        {
                            await roleManager.CreateAsync(new IdentityRole(role.ToString()));
                        }
                        if (await roleManager.RoleExistsAsync(role.ToString()))
                        {
                            await userManager.AddToRoleAsync(user, role.ToString());
                        }
                         // AUTO CONFIRM EMAIL
                        var token = userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirm = await userManager.ConfirmEmailAsync(user, token.Result);
                        if (confirm.Succeeded)
                        {
                            return new ResponeModel { Status = "Success", Message = $"Đăng ký tài khoản {role} Thành công!" };
                        }
                        foreach (var error in confirm.Errors)
                        {
                            errorMessage = error.Description;
                        }
                        return new ResponeModel { Status = "Error", Message = errorMessage };
                    }
                    return new ResponeModel { Status = "Error", Message = $"Đăng ký thất bại, role {role} không hỗ trợ bởi hệ thống!" };
                }
                return new ResponeModel { Status = "Error", Message = "Account đã tồn tại trong hệ thống!" };
            }
            catch (Exception ex)
            {
                // Log or print the exception details
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while checking if the account exists." };
            }
        }

        public Task<ResponeModel> SignUpParentAsync(SignUpModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponeModel> ConfirmEmail(string email, string token)
        {
            var user =  await userManager.FindByNameAsync(email);
            if (user.EmailConfirmed)
            {
                return new ResponeModel
                {
                    Status = "Error",
                    Message = "Email đã được xác nhận!"
                };

            }
            if (user != null)
            {
                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return new ResponeModel
                    {
                        Status = "Success",
                        Message = "Xác thực email thành công!"
                    };
                }
                return new ResponeModel
                {
                    Status = "Error",
                    Message = "Xác thực email thất bại!"
                };
            }
            return new ResponeModel
            {
                Status = "Error",
                Message = "Tài khoản không tồn tại!"
            };
        }
    }
}
