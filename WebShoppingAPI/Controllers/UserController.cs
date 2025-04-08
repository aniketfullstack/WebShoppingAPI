using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShoppingAPI.Dtos.IdentityDtos;
using WebShoppingAPI.Errors;
using WebShoppingAPI.Infrastructure.Interfaces;
using WebShoppingAPI.Infrastructure.Models.IdentityModels;

namespace WebShoppingAPI.Controllers
{
    public class UserController : BaseAPIController
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        public UserController(Microsoft.AspNetCore.Identity.UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<ActionResult<ErrorResponse>> CreateUser(RegisterUserDto registerDto)
        {
            try
            {

                if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
                {
                    return BadRequest("Email Exists");
                }

                var user = new AppUser
                {
                    DisplayName = registerDto.DisplayName,
                    Email = registerDto.Email,
                    UserName = registerDto.Email
                };
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    return new ErrorResponse() { StatusCode = 200, Message = "User Created" };
                }
                else
                {
                    return new ErrorResponse() { StatusCode = 401, Message = "Error Creating User" };
                }
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<ActionResult<UserDto>> UpdateUser(UserDto userDto)
        {
            try
            {
                var userFound = await _userManager.FindByIdAsync(userDto.UserId);
                if (userFound != null)
                {
                    userFound.DisplayName = userDto.DisplayName;
                    userFound.Email = userDto.Email;
                    userFound.UserName = userDto.UserName;
                   
                    var result = await _userManager.UpdateAsync(userFound);
                    if (result.Succeeded)
                    {
                        var updateAppUser = await _userManager.FindByIdAsync(userDto.UserId);
                        var updateUser = new UserDto
                        {
                            UserId = userFound.Id,
                            UserName = userFound.UserName,
                            Email = userFound.Email,
                            DisplayName = userFound.DisplayName,
                            Token = await _tokenService.CreateToken(userFound),
                        };

                        return Ok(updateUser);
                    }
                    else
                    {
                        return BadRequest("Problem updating User");
                    }
                }
                else
                {
                    return BadRequest("Error: User Not Foun");
                }

            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<ActionResult<ErrorResponse>> DeleteUser(string Id)
        {
            var appUser = await _userManager.FindByIdAsync(Id);
            var result = await _userManager.DeleteAsync(appUser);
            if (result.Succeeded)
            {
                return new ErrorResponse() { StatusCode = 200, Message = "User Deleted" };
            }
            else
            {
                return new ErrorResponse() { StatusCode = 401, Message = "User Delete Failed" };
            }
        }

        [Authorize(Roles = "SuperUser")]
        [HttpGet]
        [Route("GetUsers")]
        public ActionResult<IList<UserDto>> GetUsers()
        {
            var customers = new List<UserDto>();

            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                customers.Add(new UserDto
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                });
            }
            return Ok(customers);
        }

        [Authorize(Roles = "SuperUser")]
        [HttpGet]
        [Route("GetUserByUsername")]
        public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
        {
            var appUser = await _userManager.FindByNameAsync(username);
            var userDto = new UserDto
            {
                UserId = appUser.Id,
                UserName = appUser.UserName,
                Email = appUser.Email,
                DisplayName = appUser.DisplayName,
            };

            return Ok(userDto);
        }


        [Authorize(Roles ="SuperUser")]
        [HttpGet]
        [Route("GetUserById")]
        public async Task<ActionResult<UserDto>> GetUserById(string Id)
        {
            var appUser = await _userManager.FindByIdAsync(Id);

            var userDto = new UserDto
            {
                UserId = appUser.Id,
                UserName = appUser.UserName,
                Email = appUser.Email,
                DisplayName = appUser.DisplayName,
            };

            return Ok(userDto);
        }

        [Authorize(Roles = "SuperUser")]
        [HttpGet]
        [Route("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}
