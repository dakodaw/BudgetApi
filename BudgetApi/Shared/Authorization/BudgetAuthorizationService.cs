using Budget.DB.BudgetingGroups;
using BudgetApi.Users.Services;
using System.Linq;

namespace BudgetApi.Shared.Authorization;

public class BudgetAuthorizationService: IBudgetAuthorizationService
{
    private readonly IUsersService _usersService;
    private readonly IUserBudgetingGroupProvider _userBudgetingGroupProvider;
    public BudgetAuthorizationService(IUsersService usersService, IUserBudgetingGroupProvider userBudgetingGroupProvider)
    {
        _usersService = usersService;
        _userBudgetingGroupProvider = userBudgetingGroupProvider;
    }

    public bool IsUserSystemAdmin(string externalLoginId)
    {
        var user = _usersService.GetUserByExternalId(externalLoginId);
        return user.IsSystemAdmin;
    }

    public bool IsUserGroupAdmin(string externalLoginId, int groupId)
    {
        var user = _usersService.GetUserByExternalId(externalLoginId);
        var userGroups = _userBudgetingGroupProvider.GetUserBudgetingGroupsByUserId(user.Id);
        var userGroup = userGroups.FirstOrDefault(x =>  x.GroupId == groupId);

        if (userGroup == null)
            return false;

        return userGroup.IsGroupAdmin;
    }
     
    public bool IsUserInGroup(string externalLoginId, int groupId)
    {
        var user = _usersService.GetUserByExternalId(externalLoginId);
        var userGroups = _userBudgetingGroupProvider.GetUserBudgetingGroupsByUserId(user.Id);
        var userGroup = userGroups.FirstOrDefault(x => x.GroupId == groupId);

        return userGroup != null;
    }
}
