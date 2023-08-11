using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MoodSensingApp.DatabaseContext;
using MoodSensingApp.Models;
using MoodSensingApp.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MoodSensingApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private IConfiguration _config;
      

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _config = configuration;
        }

        public async Task<bool> RegisterUser(string username, string password)
        {
            try
            {
                byte[] salt = GenerateSalt();
                byte[] passwordHash = HashPassword(password, salt);

                // Convert byte arrays to Base64 strings
                string passwordHashBase64 = Convert.ToBase64String(passwordHash);
                string saltBase64 = Convert.ToBase64String(salt);

                // Create the user and set the password hash and salt
                var user = new User
                {
                    Username = username,
                    Password = passwordHashBase64,
                    Salt = saltBase64
                };

                // Save the user to the database 
                await _userRepository.AddAsync(user);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
       
        }

        public async Task<bool> AuthenticateUser(string username, string password )
        {
            var user = await _userRepository.GetByUsername(username);

            if (user == null)
            {
                return false;
            }

            byte[] salt = Convert.FromBase64String(user.Salt);
            byte[] passwordHash = HashPassword(password, salt);
            string passwordHashBase64 = Convert.ToBase64String(passwordHash);

            if (passwordHashBase64 == user.Password)
            {
                return  true;
            }
            return false; ;
        }

        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public byte[] HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(32); // 32 bytes = 256 bits
            }
        }

        public async Task<string> GenerateJSONWebToken(string userName)
        {
            var user = await _userRepository.GetByUsername(userName);

            if (user == null)
            {
                throw new Exception("User not found.");
            }
            var tokenHandler = new JwtSecurityTokenHandler();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                 {
                    new Claim("UserId", user.UserId.ToString()),
                   
                 }),

                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = credentials,
              
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }


  

}
