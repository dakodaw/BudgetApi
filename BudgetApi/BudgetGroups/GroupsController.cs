using Budget.Models;
using BudgetApi.BudgetGroups.Services;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

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
        [Route("userInfo")]
        public IEnumerable<object> GetUserInfo()
        {
            var claimsUser = HttpContext.User.Claims;
            var externalLoginUserId = claimsUser.FirstOrDefault(x => x.Type == "user_id")?.Value;
            //return claimsUser.Claims;
            return [externalLoginUserId];
        }

        [HttpGet]
        [Route("{id}")]
        public BudgetingGroup Get(int id)
        {
            return _groupsService.Get(id);
        }

        [HttpGet]
        [Route("list")]
        public ActionResult<IEnumerable<BudgetingGroup>> GetByExternalLoginId()
        {
            if (!_authorizationService.IsUserSystemAdmin(ExternalLoginId))
                return Unauthorized();

            return Ok(_groupsService.GetByExternalLoginId(ExternalLoginId));
        }

        [HttpPost]
        [Route("")]
        public int Add(BudgetingGroup budgetGroup)
        {
            return _groupsService.Add(budgetGroup);
        }

        [HttpPut]
        [Route("")]
        public void Update(BudgetingGroup budgetGroup)
        {
            _groupsService.Update(budgetGroup);
        }

        [HttpDelete]
        [Route("{id}")]
        public void Delete(int id)
        {
            _groupsService.Delete(id);
        }

        // User Groups
        [HttpPost]
        [Route("userGroup")]
        public void AddUserToGroup(AddUserToGroupRequest request)
        {
            _groupsService.AddUserToGroup(request);
        }

        [HttpDelete]
        [Route("userGroup/{groupId}/{externalLoginId}")]
        public void RemoveUserFromGroup(int groupId, string externalLoginId)
        {
            _groupsService.RemoveUserFromGroup(groupId, externalLoginId);
        }

        private string ExternalLoginId => HttpContext
            .User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
    }
}
