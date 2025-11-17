using SmartCareBLL.DTOS.AuthDTOS;
using SmartCareDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCareBLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(string Token , int UserID)> LoginAsync(string email, string password);
        public Task<User> RegisterAsync(string firstName, string lastName, string email, string password, string gender, DateOnly dateOfBirth , string phoneNumber);
    }
}
