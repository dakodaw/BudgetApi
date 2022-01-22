﻿using BudgetApi.Models;
using BudgetApi.Shared;
using System;
using System.Collections.Generic;

namespace BudgetApi.Purchases.Services
{
    public interface IPurchasesService
    {
        public List<PurchaseLine> GetPurchaseLines(DateTime monthYear);
        public bool AddUpdatePurchase(Purchase inputPurchase, int purchaseId = -1);
        public bool DeletePurchaseEntry(int purchaseId);
        public PurchaseLine GetExistingPurchase(int purchaseId);
    }
}
