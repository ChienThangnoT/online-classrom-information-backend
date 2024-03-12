using LMSystem.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LMSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseController : Controller
    {
        private readonly IFirebaseRepository _firebaseRepository;

        public FirebaseController(IFirebaseRepository firebaseRepository)
        {
            _firebaseRepository = firebaseRepository;
        }

        [HttpPost("SendMessageCloud")]
        public async Task<IActionResult> PushNotificationFirebase(string title, string body, string token)
        {
             var notification = await _firebaseRepository.PushNotificationFireBaseToken(title, body, token);
            return Ok(notification);
        }
    }
}
