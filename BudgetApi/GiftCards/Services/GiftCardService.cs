using BudgetApi.Budgeting.Models;
using BudgetApi.BudgetTypes;
using BudgetApi.GiftCards.Models;
using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using BudgetApi.Shared;
using BudgetApi.Shared.Custom;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApi.GiftCards.Services
{
    public class GiftCardService: IGiftCardService
    {
        BudgetEntities _db;
        public GiftCardService(BudgetEntities db)
        {
            _db = db;
        }
        // GET api/<controller>
        public List<GiftCardSelectLine> GetGiftCardLines()
        {
            var giftCardLines = new List<GiftCardSelectLine>();
            foreach (var giftCard in _db.GiftCards)
            {
                var remaining = GetGiftCardBalance(giftCard.Id);
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
            return giftCardLines.OrderBy(i => i.Place).ToList();
        }

        public List<GiftCardSelectLine> GetGiftCardLinesIncludingZeros()
        {
            var giftCardLines = new List<GiftCardSelectLine>();
            foreach (var giftCard in _db.GiftCards)
            {
                var remaining = GetGiftCardBalance(giftCard.Id);
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

        public decimal GetGiftCardBalance(int giftCardId)
        {
            var history = _db.Purchases.Where(i => i.GiftCardId == giftCardId).ToList();
            var initialBalance = _db.GiftCards.Where(i => i.Id == giftCardId).FirstOrDefault().InitialAmount;
            decimal currentBalance = initialBalance;
            foreach (var purchase in history)
            {
                currentBalance = currentBalance - purchase.Amount;
            }
            return currentBalance;
        }

        public List<PurchaseLine> GetPurchaseLines(DateTime monthYear)
        {
            var purchases = (from p in _db.Purchases.Where(i => i.PaymentType == PurchaseTypeNames.GiftCard && i.Date.Month == monthYear.Date.Month && i.Date.Year == monthYear.Year)
                             join t in _db.BudgetTypes on p.PurchaseTypeId equals t.Id
                             select new PurchaseLine
                             {
                                 PurchaseType = new BudgetType
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

        public GiftCardHistoryBalance GetBalanceAndHistory(int giftCardId)
        {
            return new GiftCardHistoryBalance
            {
                Balance = GetGiftCardBalance(giftCardId),
                History = _db.Purchases.Where(i => i.GiftCardId == giftCardId).ToList()
            };
        }

        public bool AddUpdateGiftCard(GiftCard inputGiftCard, int giftCardId = -1)
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

        public bool DeleteGiftCardEntry(int giftCardId)
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
        }

        public List<GiftCardHistoryBalance> GetAllBalanceAndHistory()
        {
            var balance = new List<GiftCardHistoryBalance>();
            foreach (var giftCard in _db.GiftCards)
            {
                balance.Add(new GiftCardHistoryBalance
                {
                    Balance = GetGiftCardBalance(giftCard.Id),
                    History = _db.Purchases.Where(i => i.GiftCardId == giftCard.Id).ToList(),
                    Place = giftCard.Place,
                    CardNo = giftCard.CardNumber,
                    AccessCode = giftCard.AccessCode
                });
            }
            return balance;
        }

        public GiftCard GetGiftCard(int giftCardId)
        {
            return _db.GiftCards.Where(i => i.Id == giftCardId).FirstOrDefault();
        }
    }
}
