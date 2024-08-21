
using Budget.Models;
using System.Collections.Generic;

namespace BudgetApi.Users.Services;

public interface IUsersService
{
    User GetUserByExternalId(string externalLoginId);
    int AddUser(User user);
    void UpdateUser(User user);
    void DeleteUser(User user);
    IEnumerable<User> GetUsers();
}
