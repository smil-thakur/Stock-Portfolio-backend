using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleAPI.DTOs.Account;
using SampleAPI.Interfaces;
using SampleAPI.Models;

namespace SampleAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ITockenService tockenService) : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly ITockenService _tokenservice = tockenService;
        private readonly SignInManager<AppUser> _signinManger = signInManager;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null)
            {
                return Unauthorized("Invalid Username");
            }
            var result = await _signinManger.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized("Username not found and/or Password incorrect");
            }

            return Ok(new NewUserDto { Username = user.UserName!, Email = user.Email!, tocken = _tokenservice.CreateTocken(user) });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.EmailAddress,
                };

                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(new NewUserDto { Username = appUser.UserName, Email = appUser.Email, tocken = _tokenservice.CreateTocken(appUser) });
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}