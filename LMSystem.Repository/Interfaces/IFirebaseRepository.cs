using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Interfaces
{
    public interface IFirebaseRepository
    {
        Task<string> PushNotificationFireBase(string title, string body, string accountId);
        Task<string> PushNotificationFireBaseToken(string title, string body, string token);
    }
}
