using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthenticationService(UserManager<ApplicationUser> userManager,IConfiguration configuration,IEmailSender emailSender,SignInManager<ApplicationUser> signInManager,ITokenService tokenService)
        {
            _userManager=userManager;
            _configuration=configuration;
            _emailSender=emailSender;
            _signInManager=signInManager;
            _tokenService=tokenService;
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginRequest.Email);
                if (user is null)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Invalid Email",
                    };
                }

                if (await _userManager.IsLockedOutAsync(user)) 
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Account is locked, try again later!",
                    };
                }


                var res = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password,true);
                if (res.IsLockedOut)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Account is locked duo to multiple attempts",
                    };
                }

                else if (res.IsNotAllowed)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "please confirm your email",
                    };
                }

                if(!res.Succeeded)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "invalid password",
                    };
                }

                var accessToken = await _tokenService.GenerateAccessToken(user);
                var refreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                await _userManager.UpdateAsync(user);
                return new LoginResponse()
                {
                    Success = true,
                    Message = "Login Succesfully",
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };

            }
            catch (Exception ex)
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "An Unexpexted Error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            try
            {
                var user = registerRequest.Adapt<ApplicationUser>();
                var result = await _userManager.CreateAsync(user, registerRequest.Password);
                if (!result.Succeeded)
                {
                    return new RegisterResponse()
                    {
                        Success = false,
                        Message = "User Creation Failed",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }
                await _userManager.AddToRoleAsync(user, "User");
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = Uri.EscapeDataString(token);
                var emailUrl = $"https://localhost:7292/api/auth/Account/ConfirmEmail?Token={token}&userId={user.Id}";
                await _emailSender.SendEmailAsync(user.Email, "welcome", $"<h1>welcome .. {user.UserName}</h1>" +
                    $"<a href='{emailUrl}' > confirm email </a>");
                return new RegisterResponse()
                {
                    Success = true,
                    Message = "Success"
                };
            }
            catch (Exception ex) {
                return new RegisterResponse()
                {
                    Success = false,
                    Message = "An Unexpexted Error",
                    Errors = new List<string> {ex.Message}
                };
            }
        }
        public async Task<bool> ConfirmEmailAsync(String token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) { return false; }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if(!result.Succeeded) { return false; };
            return true;
        }

        public async Task<ForgetPasswordResponse> RequestPasswordReset (ForgetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return new ForgetPasswordResponse
                {
                    Success = false,
                    Message = "Email Not Found"
                };
            }

            var random = new Random();
            var code = random.Next(1000, 9999).ToString();

            user.CodeResetPassword = code;
            user.PasswordResetCodeExpiry = DateTime.UtcNow.AddMinutes(15);

            await _userManager.UpdateAsync(user);

            await _emailSender.SendEmailAsync(
                request.Email,
                "Reset Password",
                $"<p>Your reset code is: <strong>{code}</strong></p>"
            );

            return new ForgetPasswordResponse
            {
                Success = true,
                Message = "Code sent to your email"
            };
        }

        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "Email Not Found"
                };
            }

            else if(user.CodeResetPassword != request.Code)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "Invalid Code"
                };
            }

            else if (user.PasswordResetCodeExpiry < DateTime.UtcNow)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "Code Expired"
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            if (!result.Succeeded)
            {
                return new ResetPasswordResponse
                {
                    Success = false,
                    Message = "Password reset failed",
                    Errors = result.Errors.Select(e=> e.Description).ToList()
                };
            }

            await _emailSender.SendEmailAsync(
                request.Email,
                "Change Password",
                $"<p>Your reset code is: <strong>Your Password is changed</strong></p>"
            );

            return new ResetPasswordResponse
            {
                Success = true,
                Message = "Password reset succesfully"
            };
        }

    
    
        public async Task<LoginResponse> RefreshTokenAsync(TokenApiModel request)
        {
            string accessToken = request.AccessToken;
            string refreshToken = request.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var userName = principal.Identity.Name; //this is mapped to the Name claim by default
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if( user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Invalid Client Request"
                };
            }
            var newAccessToken = await _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new LoginResponse
            {
                Success = true,
                Message = "Token Refreshed",
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}


