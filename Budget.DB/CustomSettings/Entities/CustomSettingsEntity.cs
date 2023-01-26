using System.ComponentModel.DataAnnotations;

namespace BudgetApi.Shared
{
    public class CustomSettingsEntity
    {
        [Key]
        public int Id { get; set; }
        public string KeyName { get; set; }
        public string Value { get; set; }
    }
}
