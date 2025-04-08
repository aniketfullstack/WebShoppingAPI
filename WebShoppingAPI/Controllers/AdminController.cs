using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebShoppingAPI.Dtos.IdentityDtos;
using WebShoppingAPI.Errors;
using WebShoppingAPI.Infrastructure.Models.IdentityModels;

namespace WebShoppingAPI.Controllers
{

    [ApiController]
    public class AdminController : BaseAPIController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public AdminController(UserManager<AppUser> userManager,
             RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Policy = "HighLevelAccess")]
        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) return BadRequest("Role name is required");
            try
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);

                if (roleExist)
                {
                    return BadRequest("Role already exist");
                }
                var appRole = new AppRole
                {
                    Name = roleName
                };

                var roleResult = await _roleManager.CreateAsync(appRole);

                if (roleResult.Succeeded)
                {
                    return Ok(new { message = "Role Created successfully" });
                }

                return BadRequest("Role creation failed.");


            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [Authorize(Policy = "HighLevelAccess")]
        [HttpPut]
        [Route("UpdateRole")]
        public async Task<IActionResult> UpdateRole(string roleId, string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) return BadRequest("Role name is required");

            if (string.IsNullOrEmpty(roleId)) return BadRequest("Role Id is required");

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null)
            {
                return NotFound("Role not found.");
            }

            role.Name = roleName;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return Ok(new { message = "Role Updated successfully." });
            }

            return BadRequest("Role Updation failed.");
        }

        [Authorize(Policy = "HighLevelAccess")]
        [HttpDelete]
        [Route("DeleteRole")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            if (string.IsNullOrEmpty(roleId)) return BadRequest("Invalid role");
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role is null)
                {
                    return NotFound("Role not found.");
                }

                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return Ok(new { message = "Role deleted successfully." });
                }

                return BadRequest("Role deletion failed.");
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [Authorize(Policy = "AdminLevelAccess")]
        [HttpGet]
        [Route("GetRoles")]
        public ActionResult<List<IdentityRole>> GetRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        [Authorize(Policy = "AdminLevelAccess")]
        [HttpGet("GetUsersWithRoles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = _userManager.Users.ToList();
            var result = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    Roles = roles
                });
            }

            return Ok(result);
        }

        [Authorize(Policy = "AdminLevelAccess")]
        [HttpGet("GetRolesWithUsers")]
        public async Task<ActionResult> GetRolesWithUsers()
        {
            var roles = _roleManager.Roles.ToList();
            var result = new List<object>();

            foreach (var role in roles)
            {
                var users = await _userManager.GetUsersInRoleAsync(role.Name);
                result.Add(new
                {
                    RoleId = role.Id,
                    Role = role.Name,
                    Users = users.Select(u => new { u.Id, u.UserName }),
                    UsersCount = users.Count(),
                });
            }
            return Ok(result);
        }

        [Authorize(Policy = "AdminLevelAccess")]
        [HttpGet("GetUsersByRoleId")]
        public async Task<ActionResult> GetUsersByRoleId(string roleId)
        {
            if (string.IsNullOrEmpty(roleId)) return BadRequest("Provide roleId");
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                return BadRequest("Role Not Found");

            }
            var userList = _userManager.Users;
            var roleUsers = new RolesDto
            {
                Id = role.Id,
                Name = role.Name
            };
            foreach (var user in userList)
            {
                var userExists = await _userManager.IsInRoleAsync(user, role.Name);
                if (userExists)
                {
                    roleUsers?.Users?.Add(new User()
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        DisplayName = user.DisplayName
                    });
                }

            }
            return Ok(roleUsers);
        }

        [Authorize(Policy = "HighLevelAccess")]
        [HttpPost]
        [Route("AssignRole")]
        public async Task<ActionResult> AssignRole(AssignRoleDto assignRoleDto)
        {
            if (string.IsNullOrEmpty(assignRoleDto.RoleId) || string.IsNullOrEmpty(assignRoleDto.UserId)) return BadRequest("Provide roleId and UserId");

            var role = await _roleManager.FindByIdAsync(assignRoleDto.RoleId);
            var user = await _userManager.FindByIdAsync(assignRoleDto.UserId);
            if (role is null || user is null)
            {
                return NotFound("User or Role Not Found");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var result = new IdentityResult();
            if (userRoles.Count > 0)
            {

                result = await _userManager.RemoveFromRoleAsync(user, userRoles[0]);
                if (!result.Succeeded) return BadRequest("Failed to unassign existing role");
            }
            result = await _userManager.AddToRoleAsync(user, role.Name);

            if (!result.Succeeded)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = result.Errors.FirstOrDefault()!.Description
                });
            }
            return Ok(result);
        }


        [Authorize(Policy = "HighLevelAccess")]
        [HttpPost]
        [Route("UnassignRole")]
        public async Task<IActionResult> UnassignRole(AssignRoleDto assignRoleDto)
        {
            if (string.IsNullOrEmpty(assignRoleDto.RoleId) || string.IsNullOrEmpty(assignRoleDto.UserId)) return BadRequest("Provide roleId and UserId");

            var role = await _roleManager.FindByIdAsync(assignRoleDto.RoleId);
            var user = await _userManager.FindByIdAsync(assignRoleDto.UserId);

            if (role is null || user is null)
            {
                return NotFound("User or Role Not Found");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Contains(role.Name))
            {
                var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    return BadRequest("Error unassinging role");
                }
                else
                {
                    return Ok(result);
                }
            }
            else
            {
                return BadRequest("Role not assigned to user");
            }
        }
    }
}
