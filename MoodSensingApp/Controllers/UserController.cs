using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoodSensingApp.RequestModels;
using MoodSensingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MoodSensingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Regiter the user with this api
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool registrationResult = await _userService.RegisterUser(model.Username, model.Password);

            if (registrationResult)
            {
                return Ok("User registration successful.");
            }

            return StatusCode(500, "User registration failed.");
        }

        [HttpGet]
        public  IActionResult MakeTea() {

            Task t = new Task(() => BoilingWater());
            Task t2 = new Task(() => AddSugar());
            return Ok();

        }

        public async void BoilingWater()
        {

         await  Task.Delay(10000);
            Console.WriteLine("Boiling water");

        }
        public async void AddSugar()
        {
          await  Task.Delay(1000);
            Console.WriteLine("sugar added");
        }


        /// <summary>
        /// API endpoint responsible for 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            bool isAuthenticated = await _userService.AuthenticateUser(model.UserName, model.Password);

            if (isAuthenticated )
            {
                string token = await _userService.GenerateJSONWebToken(model.UserName);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid username or password.");
        }






    }
}
