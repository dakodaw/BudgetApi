﻿using BudgetApi.Models;

namespace Budget.Models;

public class ReceiptRecordGroup
{
    public Guid? Id { get; set; }
    public Guid ReceiptRecordId { get; set; }
    public int BudgetTypeId { get; set; }
    public decimal Sum { get; set; }
    public IEnumerable<Purchase> Purchases { get; set; } = Enumerable.Empty<Purchase>();
}
