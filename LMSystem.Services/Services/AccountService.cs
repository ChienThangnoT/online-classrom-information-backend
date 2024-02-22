using LMSystem.Repository.Data;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using LMSystem.Repository.Repositories;
using LMSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;
        public AccountService(IAccountRepository repo) 
        {
            _repo = repo;            
        }

        public Task<ResponeModel> ChangePasswordAsync(ChangePasswordModel model)
        {
            var result = _repo.ChangePasswordAsync(model);
            return result;
        }

        public async Task<AccountModel> GetAccountByEmail(string email)
        {
            var result = await _repo.GetAccountByEmail(email);
            return result;
        }

        public async Task<Account> GetAccountById(string id)
        {
            var result = await _repo.GetAccountById(id);
            return result;
        }

        public async Task<AuthenticationResponseModel> RefreshToken(TokenModel tokenModel)
        {
            var result = await _repo.RefreshToken(tokenModel);
            return result;
        }

        public async Task<AuthenticationResponseModel> SignInAccountAsync(SignInModel model)
        {
            var result = await _repo.GetAccountByEmail(model.AccountEmail);
            if (result != null)
            {
                var signIn = await _repo.SignInAccountAsync(model);
                return signIn;
            }
            return new AuthenticationResponseModel
            {
                Status = false,
                Message = "Your Account not valid! Please Sign Up to Connect with LM.",
                JwtToken = null,
                Expired = null
            };
        }

        public async Task<ResponeModel> SignUpAccountAsync(SignUpModel model)
        {
            var result = await _repo.SignUpAccountAsync(model);
            return result;
        }

        public async Task<ResponeModel> SignUpAdminStaffAsync(SignUpModel model, RoleModel role)
        {
            var result = await _repo.SignUpAdminStaffAsync(model, role);
            return result;
        }

        public Task<AccountModel> UpdateAccountByEmail(AccountModel account)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponeModel> UpdateAccountProfile(UpdateProfileModel updateProfileModel, string accountId)
        {
            return await _repo.UpdateAccountProfile(updateProfileModel,accountId);    
        }

        public async Task<IEnumerable<Account>> ViewAccountList()
        {
            return await _repo.ViewAccountList();
        }
    }
}
