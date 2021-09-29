using Expo.Server.Client;
using Expo.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pushNotificationAPI.Data;
using pushNotificationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pushNotificationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {

        private readonly ILogger<NotificationsController> _logger;
        private readonly ApplicationDbContext _context;

        public NotificationsController(ILogger<NotificationsController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet("GetAllRegisterationToken")]
        public async Task<IActionResult> GetAllRegisterationToken()
        {
            try
            {
                List<User> users = await _context.Users.ToListAsync();
              
                return Ok(users);

            }
            catch (Exception e)
            {
                return BadRequest();
            }


        }

        [HttpPost("InsertRegisterationToken")]
        public async Task<IActionResult> InsertRegisterationToken(string Token)
        {
            var existToken =await _context.Users.Where(x => x.RegisterationToken.Equals(Token)).FirstOrDefaultAsync();
            if (existToken==null)
            {
                User user = new User()
                {
                    RegisterationToken = Token
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }
        [HttpPost("SendPushNotification")]
        public async Task<IActionResult> SendPushNotification(string Token,string message)
        {

            if(string.IsNullOrEmpty(Token))
            {

                List<string> registrationTokens = await _context.Users.Select(x => x.RegisterationToken).ToListAsync();
                var expoSDKClient = new PushApiClient();
                var pushTicketReq = new PushTicketRequest()
                {
                    PushTo = registrationTokens,
                    PushBadgeCount = 7,
                    PushBody = message
                };
                var result = await expoSDKClient.PushSendAsync(pushTicketReq);

                if (result?.PushTicketErrors?.Count() > 0)
                {
                    foreach (var error in result.PushTicketErrors)
                    {
                        Console.WriteLine($"Error: {error.ErrorCode} - {error.ErrorMessage}");
                    }
                    return BadRequest();
                }
                return Ok(registrationTokens);

            }
            else
            {
                var expoSDKClient = new PushApiClient();
                var pushTicketReq = new PushTicketRequest()
                {
                    PushTo = new List<string>() { Token },
                    PushBadgeCount = 7,
                    PushBody =message
                };
                var result = await expoSDKClient.PushSendAsync(pushTicketReq);

                if (result?.PushTicketErrors?.Count() > 0)
                {
                    foreach (var error in result.PushTicketErrors)
                    {
                        Console.WriteLine($"Error: {error.ErrorCode} - {error.ErrorMessage}");
                    }
                    return BadRequest();
                }
                return Ok(Token);

            }

          
        }
        [HttpDelete("DeleteAllRegisterationTokens")]
        public async Task<IActionResult> DeleteAllRegisterationTokens()
        {

            try
            {
                List<User> users = await _context.Users.ToListAsync();
                if(users.Count>0)
                {

                     _context.Users.RemoveRange(users);
                    await _context.SaveChangesAsync();
                }
                return Ok();

            }
            catch (Exception e) {
                return BadRequest();
            }
          
        }
    }
}
