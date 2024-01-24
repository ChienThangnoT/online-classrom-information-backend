using LMSystem.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<AuthenticationResponseModel> SignInAccountAsync(SignInModel model);
        public Task<ResponeModel> SignUpAccountAsync(SignUpModel model);

        public Task<AccountModel> GetAccountByEmail(string email);

        public Task<AccountModel> UpdateAccountByEmail(AccountModel account);
        public Task<AuthenticationResponseModel> RefreshToken(TokenModel tokenModel);
        public Task<ResponeModel> ChangePasswordAsync(ChangePasswordModel model);
    }
}
