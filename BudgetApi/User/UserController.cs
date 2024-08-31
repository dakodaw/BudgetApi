using Budget.Models;
using Budget.Models.ExceptionTypes;
using BudgetApi.Shared;
using BudgetApi.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApi.Settings
{
    [Authorize]
    [ApiController]
    [Route("group/{groupId}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUsersService _usersSerice;
        private readonly IBudgetAuthorizationService _authorizationService;

        public UserController(IUsersService usersService, IBudgetAuthorizationService authorizationService)
        {
            _usersSerice = usersService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<User> GetUserByExternalId(int groupId, string externalLoginId)
        {
            try
            {
                if (!(_authorizationService.IsUserGroupAdmin(ExternalLoginId, groupId)
                    || _authorizationService.IsUserSystemAdmin(ExternalLoginId)))
                {
                    return Unauthorized();
                }

                return _usersSerice.GetUserByExternalId(externalLoginId);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("list")]
        public ActionResult<IEnumerable<User>> GetUsers(int groupId)
        {
            try
            {
                if (!(_authorizationService.IsUserGroupAdmin(ExternalLoginId, groupId)
                    || _authorizationService.IsUserSystemAdmin(ExternalLoginId)))
                {
                    return Unauthorized();
                }

                return Ok(_usersSerice.GetUsers());
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("")]
        public ActionResult<int> AddUser(int groupId, [FromBody] User user)
        {
            try
            {
                if (!(_authorizationService.IsUserGroupAdmin(ExternalLoginId, groupId)
                    || _authorizationService.IsUserSystemAdmin(ExternalLoginId)))
                {
                    return Unauthorized();
                }

                return _usersSerice.AddUser(user);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Route("")]
        public ActionResult UpdateUser(int groupId, [FromBody] User user)
        {
            try
            {
                if (!(_authorizationService.IsUserGroupAdmin(ExternalLoginId, groupId)
                    || _authorizationService.IsUserSystemAdmin(ExternalLoginId)))
                {
                    return Unauthorized();
                }

                _usersSerice.UpdateUser(user);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        [Route("")]
        public ActionResult DeleteUser(int groupId, [FromBody] User user)
        {
            try
            {
                if (!(_authorizationService.IsUserGroupAdmin(ExternalLoginId, groupId)
                    || _authorizationService.IsUserSystemAdmin(ExternalLoginId)))
                {
                    return Unauthorized();
                }

                _usersSerice.DeleteUser(user);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        private string ExternalLoginId => HttpContext
            .User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
    }
}
