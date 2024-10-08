﻿using API.Data;
using API.DTO;
using API.Entity;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController(DataContext dataContext,ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) { return BadRequest("User is already exists!"); }

            return Ok();
            //using var hmac=new HMACSHA512();
            //var user = new AppUser
            //{
            //    UserName = registerDto.Username,
            //    PasswordHas = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            //    PasswordSalt = hmac.Key
            //};
            //dataContext.Add(user);
            //await dataContext.SaveChangesAsync();
            //return Ok(user);
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await dataContext.Users.FirstOrDefaultAsync(x=>x.UserName.ToLower()==loginDto.Username.ToLower());
            if (user == null) {return Unauthorized("Invalid username");}
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedhas = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedhas.Length; i++) {
                if (computedhas[i] != user.PasswordHas[i]) { return Unauthorized("Invalid Password"); }
            }
            return new UserDto()
            {
                Username = user.UserName,
                Token=tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await dataContext.Users.AnyAsync(x=>x.UserName.ToLower() == username.ToLower());
        }
    }
}
