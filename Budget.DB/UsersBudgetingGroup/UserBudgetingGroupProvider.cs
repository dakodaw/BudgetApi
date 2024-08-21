using Budget.Models;
using BudgetApi.Models;

namespace Budget.DB.BudgetingGroups;

public class UserBudgetingGroupProvider: IUserBudgetingGroupProvider
{
    BudgetEntities _db;

    public UserBudgetingGroupProvider(BudgetEntities db)
    {
        _db = db;
    }

    public Guid AddUserBudgetingGroup(UsersBudgetingGroup group)
    {
        try
        {
            var newGroup = new UsersBudgetingGroupEntity
            {
                UserId = group.UserId,
                GroupId = group.GroupId,
                IsGroupAdmin = group.IsGroupAdmin
            };

            _db.UsersBudgetingGroup.Add(newGroup);
            _db.SaveChanges();

            return newGroup.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to add Budget Type", ex);
        }
    }

    public void UpdateUserBudgetingGroup(UsersBudgetingGroup group)
    {
        try
        {
            var foundUsersBudgetingGroup = _db.UsersBudgetingGroup.Find(group);
            foundUsersBudgetingGroup.Id = group.Id;
            foundUsersBudgetingGroup.UserId = group.UserId;
            foundUsersBudgetingGroup.GroupId = group.GroupId;
            foundUsersBudgetingGroup.IsGroupAdmin = group.IsGroupAdmin;

            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update BudgetType", ex);
        }
    }

    public UsersBudgetingGroup GetUserBudgetingGroup(Guid id)
    {
        var matchingGroup = _db.UsersBudgetingGroup
            .Where(i => i.Id == id).FirstOrDefault();

        return new UsersBudgetingGroup
        {
            Id = matchingGroup.Id,
            UserId = matchingGroup.UserId,
            GroupId = matchingGroup.GroupId,
            IsGroupAdmin = matchingGroup.IsGroupAdmin
        };
    }

    public IEnumerable<UsersBudgetingGroup> GetUserBudgetingGroupsByUserId(int userId)
    {
        var matchingGroups = _db.UsersBudgetingGroup
            .Where(i => i.UserId == userId).AsEnumerable();

        return matchingGroups.Select(group => new UsersBudgetingGroup
        {
            Id = group.Id,
            UserId = group.UserId,
            GroupId = group.GroupId,
            IsGroupAdmin = group.IsGroupAdmin
        });
    }

    public IEnumerable<UsersBudgetingGroup> GetUserBudgetingGroups()
    {
        return _db.UsersBudgetingGroup.Select(x => new UsersBudgetingGroup
        {
            Id = x.Id,
            UserId = x.UserId,
            GroupId = x.GroupId,
            IsGroupAdmin = x.IsGroupAdmin
        });
    }

    public void DeleteUserBudgetingGroup(Guid id)
    {
        try
        {
            var toDelete = _db.UsersBudgetingGroup.Find(id);
            _db.UsersBudgetingGroup.Remove(toDelete);
            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to Delete Budget Type", ex);
        }
    }
}
