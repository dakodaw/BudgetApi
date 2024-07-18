//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BudgetApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

<<<<<<<< HEAD:Budget.DB/Incomes/Entities/IncomeSourceEntity.cs
    public partial class IncomeSourceEntity
========
    public class IncomeSourceEntity
>>>>>>>> main:Budget.Models/IncomeSourceEntity.cs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IncomeSourceEntity()
        {
            this.Incomes = new HashSet<IncomeEntity>();
        }

        [Key]
        public int Id { get; set; }
        public string SourceName { get; set; }
        public string JobOf { get; set; }
        public string PositionName { get; set; }
        public bool ActiveJob { get; set; }
        public decimal? EstimatedIncome { get; set; }
        public string PayFrequency { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncomeEntity> Incomes { get; set; }
    }
}
