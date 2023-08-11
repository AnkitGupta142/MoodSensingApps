using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodSensingApp.Services
{
    public interface IUserService
    {
      Task<bool> RegisterUser(string username, string password);

        Task<bool> AuthenticateUser(string username, string password);
        Task<string> GenerateJSONWebToken(string userName);
    }
}
