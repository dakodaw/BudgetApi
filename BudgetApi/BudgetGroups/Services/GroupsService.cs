using Budget.DB.BudgetingGroups;
using Budget.Models;
using BudgetApi.Users.Services;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApi.BudgetGroups.Services
{
    public class GroupsService: IGroupsService
    {
        IBudgetingGroupProvider _groupProvider;
        IUserBudgetingGroupProvider _userBudgetingGroupProvider;
        IUsersService _usersService;
        public GroupsService(
            IBudgetingGroupProvider groupProvider, 
            IUserBudgetingGroupProvider userBudgetingGroupProvider,
            IUsersService usersService)
        {
            _groupProvider = groupProvider;
            _userBudgetingGroupProvider = userBudgetingGroupProvider;
            _usersService = usersService;
        }

        public BudgetingGroup Get(int id)
        {
            return _groupProvider.GetBudgetingGroup(id);
        }

        public IEnumerable<BudgetingGroup> GetByExternalLoginId(string externalLoginId)
        {
            var user = _usersService.GetUserByExternalId(externalLoginId);
            // TODO: Throw custom not found exception when user not found

            var userGroups = _userBudgetingGroupProvider.GetUserBudgetingGroupsByUserId(user.Id);

            return userGroups.Select(userGroup => _groupProvider.GetBudgetingGroup(userGroup.GroupId));
        }

        public int Add(BudgetingGroup budgetGroup)
        {
            return _groupProvider.AddBudgetingGroup(budgetGroup);
        }
        
        public void Update(BudgetingGroup budgetGroup)
        {
            _groupProvider.UpdateBudgetingGroup(budgetGroup);
        }
        
        public void Delete(int id)
        {
            _groupProvider.DeleteBudgetingGroup(id);
        }
        // Add, and Remove User from group (by removing a row from the usergroups provider)
        
        public void AddUserToGroup(AddUserToGroupRequest request)
        {
            var user = _usersService.GetUserByExternalId(request.ExternalLoginId);
            var newOrUpdatedUser = new UsersBudgetingGroup
            {
                IsGroupAdmin = request.IsGroupAdmin,
                GroupId = request.GroupId,
                UserId = user.Id
            };

            // Check for existing groups for the user
            var existingGroups = _userBudgetingGroupProvider.GetUserBudgetingGroupsByUserId(user.Id);
            if (existingGroups.Any(x => x.GroupId == request.GroupId && x.UserId == user.Id))
            {
                _userBudgetingGroupProvider.UpdateUserBudgetingGroup(newOrUpdatedUser);

                return;
            }

            _userBudgetingGroupProvider.AddUserBudgetingGroup(newOrUpdatedUser);
        }

        public void RemoveUserFromGroup(int groupId, string externalLoginId)
        {
            var user = _usersService.GetUserByExternalId(externalLoginId);
            var userGroups = _userBudgetingGroupProvider.GetUserBudgetingGroupsByUserId(user.Id);
            var matchingUserGroups = userGroups.FirstOrDefault(x => x.GroupId == groupId);
            if (matchingUserGroups != null)
                _userBudgetingGroupProvider.DeleteUserBudgetingGroup(matchingUserGroups.Id);
        }
    }
}
