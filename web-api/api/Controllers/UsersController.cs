using System;
using System.Threading.Tasks;
using api.Models;
using api.Models.Incoming;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("v1/users")]
    public class UsersController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<Response<User>> Login([FromBody]UserLogin loginData)
        {
            try {
                var loggedInUser = await _userService.Login(loginData.Email, loginData.Password);
                return Response<User>.SuccessResponse(loggedInUser);
            }
            catch(Exception e) {
                return Response<User>.ErrorResponse(e.Message);
            }
        }
    }
}