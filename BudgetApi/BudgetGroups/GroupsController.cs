using Budget.Models;
using Budget.Models.ExceptionTypes;
using BudgetApi.BudgetGroups.Services;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApi.BudgetGroups
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupsService _groupsService;
        private readonly IBudgetAuthorizationService _authorizationService;
        public GroupsController(IGroupsService groupsService, IBudgetAuthorizationService budgetAuthorizationService)
        {
            _groupsService = groupsService;
            _authorizationService = budgetAuthorizationService;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<BudgetingGroup> Get(int id)
        {
            try
            {
                if (!(_authorizationService.IsUserSystemAdmin(ExternalLoginId) 
                    || _authorizationService.IsUserInGroup(ExternalLoginId, id)))
                {
                    return Unauthorized();
                }

                return _groupsService.Get(id);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("list")]
        public ActionResult<IEnumerable<BudgetingGroup>> GetByExternalLoginId()
        {
            try
            {
                return Ok(_groupsService.GetByExternalLoginId(ExternalLoginId));
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("")]
        public ActionResult<int> Add(BudgetingGroup budgetGroup)
        {
            try
            {
                // TODO: Figure out if I want to allow people to add groups from user, or only admin to add groups
                if (_authorizationService.IsUserSystemAdmin(ExternalLoginId))
                    return Unauthorized();

                return _groupsService.Add(budgetGroup);
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpPut]
        [Route("")]
        public ActionResult Update(BudgetingGroup budgetGroup)
        {
            try
            {
                if (_authorizationService.IsUserSystemAdmin(ExternalLoginId))
                    return Unauthorized();

                _groupsService.Update(budgetGroup);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                if (_authorizationService.IsUserSystemAdmin(ExternalLoginId))
                    return Unauthorized();

                _groupsService.Delete(id);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        // User Groups
        [HttpPost]
        [Route("userGroup")]
        public ActionResult AddUserToGroup(AddUserToGroupRequest request)
        {
            try
            {
                if (_authorizationService.IsUserGroupAdmin(ExternalLoginId, request.GroupId) 
                    || _authorizationService.IsUserSystemAdmin(ExternalLoginId))
                {
                    return Unauthorized();
                }

                _groupsService.AddUserToGroup(request);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        [Route("userGroup/{groupId}/{externalLoginId}")]
        public ActionResult RemoveUserFromGroup(int groupId, string externalLoginId)
        {
            try
            {
                if (_authorizationService.IsUserGroupAdmin(ExternalLoginId, groupId)
                    || _authorizationService.IsUserSystemAdmin(ExternalLoginId))
                {
                    return Unauthorized();
                }

                _groupsService.RemoveUserFromGroup(groupId, externalLoginId);
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
