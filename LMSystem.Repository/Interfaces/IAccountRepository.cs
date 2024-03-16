﻿using LMSystem.Repository.Data;
using LMSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Interfaces
{
    public interface IAccountRepository
    {
        public Task<AuthenticationResponseModel> SignInAccountAsync(SignInModel model);

        public Task<ResponeModel> SignUpAccountAsync(SignUpModel model);
        public Task<AccountModel> GetAccountByEmail(string email);
        public Task<Account> GetAccountById(string id);
        public Task<List<string>> GetListAccountIds();
        public Task<Account> GetAccountByIdV1(string id);
        public Task<ResponeModel> GetAccountByParentAccountId(string accountId);
        public Task<AccountModel> UpdateAccountByEmail(AccountModel account);
        public Task<AuthenticationResponseModel> RefreshToken(TokenModel tokenModel);
        public Task<ResponeModel> UpdateAccountProfile(UpdateProfileModel updateProfileModel, string accountId);
        public Task<ResponeModel> ChangePasswordAsync(ChangePasswordModel changePassword);
        public Task<ResponeModel> SignUpAdminStaffAsync (SignUpModel model, RoleModel role);
        public Task<ResponeModel> SignUpParentAsync (string parentEmail);
        public Task<ResponeModel> ConfirmEmail(string email, string token);
        public Task<ResponeModel> DeleteAccount(string accountId);
        public Task<AccountListResult> ViewAccountList(AccountFilterParameters filterParams);
        public Task<bool> UpdateDeviceToken(string accountId, string deviceToken);
        public Task<ResponeModel> CountTotalStudent();
        public Task<ResponeModel> CountTotalAccount();
    }
}
