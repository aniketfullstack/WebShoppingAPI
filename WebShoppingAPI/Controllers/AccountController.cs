using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebShoppingAPI.Dtos;
using WebShoppingAPI.Errors;
using WebShoppingAPI.Extensions;
using WebShoppingAPI.Infrastructure.Interfaces;
using WebShoppingAPI.Infrastructure.Models.IdentityModels;

namespace WebShoppingAPI.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        [Route("TestAuth")]
        public ActionResult<string> TestAuth()
        {
            return "secret stuff";
        }

        [Authorize]
        [HttpGet]
        [Route("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {

            var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);

            return new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        [HttpGet]
        [Route("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [Authorize]
        [HttpGet]
        [Route("GetUserAddress")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserByClaimsPrincipalWithAddressAsync(User);

            return _mapper.Map<Address, AddressDto>(user.Address);
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateUserAddress")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto addressDto)
        {
            try
            {

                var user = await _userManager.FindUserByClaimsPrincipalWithAddressAsync(User);
                user.Address = _mapper.Map<AddressDto, Address>(addressDto);

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded) return Ok(_mapper.Map<AddressDto>(user.Address));
                return BadRequest("Problem updating address");
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
                ;
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var appUser = await _userManager.FindByEmailAsync(loginDto.Email);

            if (appUser == null)
            {
                if (appUser == null) return Unauthorized(new ApiResponse(401));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDto
            {
                UserId = appUser.Id,
                Email = appUser.Email,
                Token = await _tokenService.CreateToken(appUser),
                DisplayName = appUser.DisplayName
            };

        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterUserDto registerDto)
        {
            try
            {

                if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
                {
                    return BadRequest("Email Exists");
                }

                var newAppUser = new AppUser
                {
                    DisplayName = registerDto.DisplayName,
                    Email = registerDto.Email,
                    UserName = registerDto.Email
                };

                var result = await _userManager.CreateAsync(newAppUser, registerDto.Password);
                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(newAppUser, "RegularUser");
                    if (!roleResult.Succeeded) return BadRequest(result.Errors);

                    var newUser = new UserDto
                    {
                        UserId = newAppUser.Id,
                        UserName = newAppUser.UserName,
                        Email = newAppUser.Email,
                        DisplayName = newAppUser.DisplayName,
                        Token = await _tokenService.CreateToken(newAppUser),
                    };

                    return Ok(newUser);
                }
                else
                {
                    return BadRequest(result.Errors);
                }

            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }
    }
}
