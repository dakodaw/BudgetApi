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

public partial class BudgetEntity
{
    public int Id { get; set; }
    public int BudgetTypeId { get; set; }
    public System.DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public int BudgetingGroupId { get; set; }

    public virtual BudgetTypeEntity BudgetType { get; set; }
    public virtual BudgetingGroupEntity BudgetingGroup { get; set; }
}
