using LMSystem.Repository.Helpers;
using LMSystem.Repository.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Repositories
{
    public class FirebaseRepository : IFirebaseRepository
    {
        private readonly IAccountRepository _accountRepository;

        public FirebaseRepository(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<string> PushNotificationFireBase(string title, string body, string accountId)
        {
            try
            {
                var account = await _accountRepository.GetAccountById(accountId);
                if (account != null)
                {
                    if (account.DeviceToken != null)
                    {
                        return await FirebaseLibrary.SendMessageFireBase(title, body, account.DeviceToken);

                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> PushNotificationFireBaseToken(string title, string body, string token)
        {
            try
            {
                return await FirebaseLibrary.SendMessageFireBase(title, body, token);
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
