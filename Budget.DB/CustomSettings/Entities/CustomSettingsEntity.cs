using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget.DB;

public class CustomSettingsEntity
{
    [Key]
    public int Id { get; set; }
    public string KeyName { get; set; }
    public string Value { get; set; }
    public int? BudgetingGroupId { get; set; }

    //[ForeignKey(nameof(BudgetingGroupId))]
    //public virtual BudgetingGroupEntity BudgetingGroup { get; set; }
}
