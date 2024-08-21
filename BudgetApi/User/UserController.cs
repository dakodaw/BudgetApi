using Budget.Models;
using BudgetApi.Users.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BudgetApi.Settings
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        IUsersService _usersSerice;
        public UserController(IUsersService usersService)
        {
            _usersSerice = usersService;
        }

        [HttpGet]
        [Route("")]
        public User GetUserByExternalId(string externalLoginId)
        {
            return _usersSerice.GetUserByExternalId(externalLoginId);
        }

        [HttpGet]
        [Route("list")]
        public IEnumerable<User> GetUsers()
        {
            return _usersSerice.GetUsers();
        }

        [HttpPost]
        [Route("")]
        public int AddUser(User user)
        {
            return _usersSerice.AddUser(user);
        }

        [HttpPut]
        [Route("")]
        public void UpdateUser(User user)
        {
            _usersSerice.UpdateUser(user);
        }

        [HttpDelete]
        [Route("")]
        public void DeleteUser(User user)
        {
            _usersSerice.DeleteUser(user);
        }
    }
}
