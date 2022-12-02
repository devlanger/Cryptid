using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dto;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized("Couldnt find user");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            
            if(result.Succeeded)
            {
                return CreateUser(user);
            }

            return Unauthorized("Wrong email or password");
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = await userManager.FindByEmailAsync(registerDto.Email);
            if (user != null)
            {
                return Unauthorized("Email taken");
            }

            var registeredUser = new AppUser
            {
                Email = registerDto.Email,
                UserName = registerDto.Username,
                Nickname = registerDto.Nickname
            };

            var result = await userManager.CreateAsync(registeredUser, registerDto.Password);
            
            if(result.Succeeded)
            {
                return CreateUser(registeredUser);
            }

            return BadRequest("Something went wrong with registration...");
        }

        private UserDto CreateUser(AppUser user)
        {
            return new UserDto
            {
                Token = "",
                Username = user.UserName,
                Nickname = user.Nickname,
            };
        }
    }
}