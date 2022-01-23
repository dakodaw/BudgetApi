using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using BudgetApi.Purchases.Services;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BudgetApi.Purchases
{
    [ApiController]
    [Route("[controller]")]
    public class PurchasesController : Controller
    {
        IPurchasesService _purchasesService;
        public PurchasesController(IPurchasesService purchasesService)
        {
            _purchasesService = purchasesService;
        }

        [HttpGet]
        [Route("getPurchaseLines")]
        public List<PurchaseLine> GetPurchaseLines([FromQuery] DateTime monthYear)
        {
            return _purchasesService.GetPurchaseLines(monthYear);
        }

        [HttpPost]
        [Route("addUpdatePurchase")]
        public bool AddUpdatePurchase([FromBody] Purchase inputPurchase, [FromQuery] int purchaseId = -1)
        {
            return _purchasesService.AddUpdatePurchase(inputPurchase, purchaseId);
        }

        [HttpGet]
        [Route("deletePurchaseEntry")]
        public bool DeletePurchaseEntry([FromQuery] int purchaseId)
        {
            return _purchasesService.DeletePurchaseEntry(purchaseId);
        }

        [HttpGet]
        [Route("getExistingPurchase")]
        public PurchaseLine GetExistingPurchase([FromQuery] int purchaseId)
        {
            return _purchasesService.GetExistingPurchase(purchaseId);
        }
    }
}
