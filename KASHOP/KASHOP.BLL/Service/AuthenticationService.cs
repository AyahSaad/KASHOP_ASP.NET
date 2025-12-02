using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager)
        {
            _userManager=userManager;
        }
        public Task<LoginResponse> LoginAsync(LoginRequest LoginRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            var user = registerRequest.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            if (!result.Succeeded)
            {
                return new RegisterResponse()
                {
                    Success = false,
                    Message = "User Creation Failed",
                    Errors = result.Errors.Select(e=>e.Description).ToList()
                };
            }

            await _userManager.AddToRoleAsync(user, "User");
            return new RegisterResponse()
            {
                Success = true,
                Message = "Success"
            };
        }
    }
}
