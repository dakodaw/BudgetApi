using System.ComponentModel.DataAnnotations;

namespace Budget.DB;

public class CustomSettingsEntity
{
    [Key]
    public int Id { get; set; }
    public string KeyName { get; set; }
    public string Value { get; set; }
}
