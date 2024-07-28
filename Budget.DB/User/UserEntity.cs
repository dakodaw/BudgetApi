using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.DB.User;

public class UserEntity
{
    public int Id { get; set; }
    public string UserSSOLoginId { get; set; }
    public int GroupBudgetId { get; set; }
    public bool IsUserAdmin { get; set; }
}
