using Budget.Models;
using BudgetApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.DB.Users;

public class UserProvider: IUserProvider
{
    private BudgetEntities _db;

    public UserProvider(BudgetEntities db)
    {
        _db = db;
    }

    public int AddUser(User user)
    {
        try
        {
            var newUserEntity = new UserEntity
            {
                Id = user.Id,
                UserSSOLoginId = user.UserSSOLoginId,
                IsSystemAdmin = user.IsSystemAdmin
            };

            _db.User.Add(newUserEntity);
            _db.SaveChanges();

            return newUserEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to add User", ex);
        }
    }

    public void UpdateUser(User user)
    {
        try
        {
            var foundUser = _db.User.Find(user.Id);
            foundUser.UserSSOLoginId = user.UserSSOLoginId;
            foundUser.IsSystemAdmin = user.IsSystemAdmin;

            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update User", ex);
        }
    }

    public User GetUser(int id)
    {
        var matchingUser = _db.User
                .Where(i => i.Id == id).FirstOrDefault();

        return new User
        {
            Id = matchingUser.Id,
            UserSSOLoginId = matchingUser.UserSSOLoginId,
            IsSystemAdmin = matchingUser.IsSystemAdmin
        };
    }

    public User GetUserByExternalLoginId(string loginId)
    {
        var matchingUser = _db.User
            .FirstOrDefault(i => i.UserSSOLoginId == loginId);
        // TODO: later throw custom Exception
        if (matchingUser == null)
            throw new Exception($"No user found with the loginId {loginId}");

        return new User
        {
            Id = matchingUser.Id,
            UserSSOLoginId = matchingUser.UserSSOLoginId,
            IsSystemAdmin = matchingUser.IsSystemAdmin
        };
    }

    public IEnumerable<User> GetUsers()
    {
        return _db.User.Select(x => new User
        {
            Id = x.Id,
            UserSSOLoginId = x.UserSSOLoginId,
            IsSystemAdmin = x.IsSystemAdmin
        });
    }

    public void DeleteUser(int id)
    {
        try
        {
            var toDelete = _db.User.Find(id);
            _db.User.Remove(toDelete);
            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to Delete User", ex);
        }
    }
}
