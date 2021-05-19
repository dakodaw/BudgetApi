using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BudgetTest.Models;
using BudgetTest.Controllers;

namespace BudgetTest.Controllers
{
    public class GiftCardController : ApiController
    {
        BudgetTestEntities _db = new BudgetTestEntities();
        // GET api/<controller>
        [HttpGet]
        public List<GiftCardSelectLine> GetGiftCardLines([FromUri] bool getGiftCardLines)
        {
            var giftCardLines = new List<GiftCardSelectLine>();
            foreach(var giftCard in _db.GiftCards)
            {
                var remaining = GetGiftCardBalance(true, giftCard.Id);
                if (remaining > 0)
                {
                    var card = new GiftCardSelectLine
                    {
                        Id = giftCard.Id,
                        Place = giftCard.Place,
                        Last4ofCardNumber = giftCard.CardNumber.GetLast(4),
                        RemainingAmount = remaining
                    };
                    giftCardLines.Add(card);
                }
            }
            return giftCardLines.OrderBy(i=>i.Place).ToList();
        }

        [HttpGet]
        public List<GiftCardSelectLine> GetGiftCardLinesIncludingZeros([FromUri] bool getGiftCardLinesIncludingZeros)
        {
            var giftCardLines = new List<GiftCardSelectLine>();
            foreach (var giftCard in _db.GiftCards)
            {
                var remaining = GetGiftCardBalance(true, giftCard.Id);
                if (remaining > 0)
                {
                    var card = new GiftCardSelectLine
                    {
                        Id = giftCard.Id,
                        Place = giftCard.Place,
                        Last4ofCardNumber = giftCard.CardNumber.GetLast(4),
                        RemainingAmount = remaining
                    };
                    giftCardLines.Add(card);
                }
                else
                {
                    var card = new GiftCardSelectLine
                    {
                        Id = giftCard.Id,
                        Place = giftCard.Place,
                        Last4ofCardNumber = giftCard.CardNumber.GetLast(4),
                    };
                    giftCardLines.Add(card);
                }
            }
            return giftCardLines.OrderBy(i => i.Place).ToList();
        }

        [HttpGet]
        public decimal GetGiftCardBalance([FromUri] bool getGiftCardBalance, [FromUri] int giftCardId)
        {
            var history = _db.Purchases.Where(i => i.GiftCardId == giftCardId).ToList();
            var initialBalance = _db.GiftCards.Where(i => i.Id == giftCardId).FirstOrDefault().InitialAmount;
            decimal currentBalance = initialBalance;
            foreach(var purchase in history)
            {
                currentBalance = currentBalance - purchase.Amount;
            }
            return currentBalance;
        }

        [HttpGet]
        public List<PurchaseLine> GetPurchaseLines([FromUri] bool getPurchaseLines, [FromUri] DateTime monthYear)
        {
            var purchases = (from p in _db.Purchases.Where(i => i.PaymentType == PurchaseTypeNames.GiftCard && i.Date.Month == monthYear.Date.Month && i.Date.Year == monthYear.Year)
                             join t in _db.BudgetTypes on p.PurchaseTypeId equals t.Id
                             select new PurchaseLine
                             {
                                 PurchaseType = new BudgetTypes
                                 {
                                     BudgetTypeId = p.PurchaseTypeId,
                                     BudgetTypeName = t.BudgetType1
                                 },
                                 Description = p.Description,
                                 Date = p.Date,
                                 Amount = p.Amount,
                                 Id = p.Id,
                                 PaymentType = p.PaymentType,
                                 //GiftCardId = p.GiftCardId,
                                 IsReimbursement = p.FutureReimbursement
                             }).ToList();
            foreach (var purchase in purchases)
            {
                if (purchase.PaymentType == PurchaseTypeNames.GiftCard)
                {
                    purchase.GiftCardId = (int)_db.Purchases.Where(i => i.Id == purchase.Id).FirstOrDefault().GiftCardId;
                }
            }
            return purchases;
        }

        [HttpGet]
        public GiftCardHistoryBalance GetBalanceAndHistory([FromUri] bool getBalanceAndHistory, [FromUri] int giftCardId)
        {
            return new GiftCardHistoryBalance
            {
                Balance = GetGiftCardBalance(true, giftCardId),
                History = _db.Purchases.Where(i => i.GiftCardId == giftCardId).ToList()
            };
        }


        [HttpPost]
        public bool AddUpdateGiftCard([FromUri] bool addUpdateGiftCard, [FromBody] GiftCard inputGiftCard, [FromUri] int giftCardId = -1)
        {
            if (giftCardId == -1)
            {
                bool success = false;
                _db.GiftCards.Add(inputGiftCard);
                _db.SaveChanges();
                try
                {
                    var checkCard = _db.GiftCards.Where(i => i.CardNumber == inputGiftCard.CardNumber).FirstOrDefault();
                    success = true;
                }
                catch
                {

                }
                return success;
            }
            else
            {
                bool success = false;
                
                try
                {
                    _db.GiftCards.Where(i => i.Id == giftCardId).FirstOrDefault().AccessCode = inputGiftCard.AccessCode;
                    _db.GiftCards.Where(i => i.Id == giftCardId).FirstOrDefault().CardNumber = inputGiftCard.CardNumber;
                    _db.GiftCards.Where(i => i.Id == giftCardId).FirstOrDefault().InitialAmount = inputGiftCard.InitialAmount;
                    _db.GiftCards.Where(i => i.Id == giftCardId).FirstOrDefault().Place = inputGiftCard.Place;
                    _db.SaveChanges();
                    success = true;
                }
                catch
                {

                }
                return success;
            }
        }

        [HttpGet]
        public bool DeleteGiftCardEntry([FromUri] bool deleteGiftCardEntry, [FromUri] int giftCardId)
        {
            try
            {
                var toDelete = _db.GiftCards.Find(giftCardId);
                _db.GiftCards.Remove(toDelete);
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

        [HttpGet]
        public List<GiftCardHistoryBalance> GetAllBalanceAndHistory([FromUri] bool getAllBalanceAndHistory)
        {
            var balance = new List<GiftCardHistoryBalance>();
            foreach(var giftCard in _db.GiftCards)
            {
                balance.Add(new GiftCardHistoryBalance {
                    Balance = GetGiftCardBalance(true, giftCard.Id),
                    History = _db.Purchases.Where(i => i.GiftCardId == giftCard.Id).ToList(),
                    Place = giftCard.Place,
                    CardNo = giftCard.CardNumber,
                    AccessCode = giftCard.AccessCode
                });
            }
            return balance;
        }

        [HttpGet]
        public GiftCard GetGiftCard([FromUri] bool getGiftCard, [FromUri] int giftCardId)
        {
            return _db.GiftCards.Where(i => i.Id == giftCardId).FirstOrDefault();
        }

    }
}