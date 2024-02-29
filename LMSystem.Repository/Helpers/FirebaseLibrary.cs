using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Helpers
{
    public class FirebaseLibrary
    {
        public static async Task<string> SendMessageFireBase(string title, string body, string token)
        {
            try
            {
                var message = new Message()
                {
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    },
                    Token = token
                };

                // Send the message
                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine($"Successfully sent message: {response}");
                return response;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}
