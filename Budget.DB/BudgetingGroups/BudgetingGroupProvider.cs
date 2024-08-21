using Budget.Models;
using BudgetApi.Models;

namespace Budget.DB.BudgetingGroups;

public class BudgetingGroupProvider: IBudgetingGroupProvider
{
    BudgetEntities _db;

    public BudgetingGroupProvider(BudgetEntities db)
    {
        _db = db;
    }

    public int AddBudgetingGroup(BudgetingGroup group)
    {
        try
        {
            var newGroup = new BudgetingGroupEntity
            {
                GroupName = group.GroupName
            };

            _db.BudgetingGroup.Add(newGroup);
            _db.SaveChanges();

            return newGroup.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to add Budget Type", ex);
        }
    }

    public void UpdateBudgetingGroup(BudgetingGroup group)
    {
        try
        {
            var foundBudgetType = _db.BudgetingGroup.Find(group);
            foundBudgetType.GroupName = group.GroupName;

            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update BudgetType", ex);
        }
    }

    public BudgetingGroup GetBudgetingGroup(int id)
    {
        var matchingType = _db.BudgetingGroup
            .Where(i => i.Id == id).FirstOrDefault();

        return new BudgetingGroup
        {
            Id = matchingType.Id,
            GroupName = matchingType.GroupName
        };
    }

    // TODO: This should be done at the service level
    //public BudgetingGroup GetBudgetingGroupByExternalLoginId(string loginId)
    //{
    //    var matchingType = _db.BudgetingGroup
    //            .Where(i => i. == id).FirstOrDefault();

    //    return new BudgetingGroup
    //    {
    //        Id = matchingType.Id,
    //        GroupName = matchingType.GroupName
    //    };
    //}

    public IEnumerable<BudgetingGroup> GetBudgetingGroups()
    {
        return _db.BudgetingGroup.Select(x => new BudgetingGroup
        {
            Id = x.Id,
            GroupName = x.GroupName
        });
    }

    public void DeleteBudgetingGroup(int id)
    {
        try
        {
            var toDelete = _db.BudgetingGroup.Find(id);
            _db.BudgetingGroup.Remove(toDelete);
            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to Delete Budget Type", ex);
        }
    }
}
