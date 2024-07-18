using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using BudgetApi.Purchases.Services;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BudgetApi.Purchases
{
    [Authorize]
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
        [Route("{purchaseId}")]
        public PurchaseLine GetPurchase(int purchaseId)
        {
            return _purchasesService.GetExistingPurchase(purchaseId);
        }

        [HttpPost]
        [Route("")]
        public int AddPurchase([FromBody] Purchase incomePurchase)
        {
            return _purchasesService.AddPurchase(incomePurchase);
        }

        [HttpPut]
        [Route("{purchaseId}")]
        public void UpdatePurchase([FromBody] Purchase incomePurchase, int purchaseId)
        {
            _purchasesService.UpdatePurchase(incomePurchase);
        }

        [HttpDelete]
        [Route("{purchaseId}")]
        public bool DeletePurchase(int purchaseId)
        {
            return _purchasesService.DeletePurchaseEntry(purchaseId);
        }

        [HttpGet]
        [Route("getPurchaseLines")]
        public List<PurchaseLine> GetPurchaseLines([FromQuery] DateTime monthYear)
        {
            return _purchasesService.GetPurchaseLines(monthYear);
        }

        [Obsolete("Please use the http post and http put on the base route instead")]
        [HttpPost]
        [Route("addUpdatePurchase")]
        public bool AddUpdatePurchase([FromBody] Purchase inputPurchase, [FromQuery] int purchaseId = -1)
        {
            return _purchasesService.AddUpdatePurchase(inputPurchase, purchaseId);
        }


        [Obsolete("Please use the http delete on the base route instead")]
        [HttpGet]
        [Route("deletePurchaseEntry")]
        public bool DeletePurchaseEntry([FromQuery] int purchaseId)
        {
            return _purchasesService.DeletePurchaseEntry(purchaseId);
        }

        [Obsolete("Please use the http get on the base route instead")]
        [HttpGet]
        [Route("getExistingPurchase")]
        public PurchaseLine GetExistingPurchase([FromQuery] int purchaseId)
        {
            return _purchasesService.GetExistingPurchase(purchaseId);
        }
    }
}
