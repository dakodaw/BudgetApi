using Budget.Models;
using System.Collections.Generic;

namespace BudgetApi.BudgetGroups.Services
{
    public interface IGroupsService
    {
        BudgetingGroup Get(int id);
        IEnumerable<BudgetingGroup> GetByExternalLoginId(string externalLoginId);
        int Add(BudgetingGroup budgetGroup);
        void Update(BudgetingGroup budgetGroup);
        void Delete(int id);
        // Add, and Remove User from group (by removing a row from the usergroups provider)
        void AddUserToGroup(AddUserToGroupRequest addUserToGroupRequest);
        void RemoveUserFromGroup(int groupId, string externalLoginId);
    }
}
