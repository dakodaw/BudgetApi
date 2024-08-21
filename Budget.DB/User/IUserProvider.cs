using Budget.Models;

namespace Budget.DB.Users;

public interface IUserProvider
{
    int AddUser(User user);
    void UpdateUser(User user);
    User GetUser(int id);
    User GetUserByExternalLoginId(string loginId);
    IEnumerable<User> GetUsers();
    void DeleteUser(int id);
}
