using Budget.DB.Users;
using Budget.Models;
using System.Collections.Generic;

namespace BudgetApi.Users.Services;

public class UsersService: IUsersService
{
    private readonly IUserProvider _userProvider;
    public UsersService(IUserProvider userProvider)
    {
        _userProvider = userProvider;
    }

    public User GetUserByExternalId(string externalLoginId)
    {
        return _userProvider.GetUserByExternalLoginId(externalLoginId);
    }

    public int AddUser(User user)
    {
        return _userProvider.AddUser(user);
    }

    public void UpdateUser(User user)
    {
        _userProvider.UpdateUser(user);
    }

    public void DeleteUser(User user)
    {
        _userProvider.DeleteUser(user.Id);
    }

    public IEnumerable<User> GetUsers()
    {
        return _userProvider.GetUsers();
    }
}
