using System.ComponentModel.DataAnnotations;

namespace BudgetApi.Shared
{
    public class CustomSettings
    {
        [Key]
        public int Id { get; set; }
        public string KeyName { get; set; }
        public string Value { get; set; }
    }
}
