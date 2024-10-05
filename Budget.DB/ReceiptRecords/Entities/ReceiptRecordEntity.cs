//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Budget.DB;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public partial class ReceiptRecordEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public int BudgetingGroupId { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Location { get; set; }

    //[ForeignKey("SourceId")]
    //public virtual IncomeSourceEntity IncomeSource { get; set; }
}
