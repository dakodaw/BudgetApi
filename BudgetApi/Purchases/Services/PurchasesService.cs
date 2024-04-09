﻿using BudgetApi.Budgeting.Models;
using BudgetApi.BudgetTypes;
using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using BudgetApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApi.Purchases.Services
{
    public class PurchasesService: IPurchasesService
    {
        BudgetEntities _db;
        public PurchasesService(BudgetEntities db)
        {
            _db = db;
        }

        public List<PurchaseLine> GetPurchaseLines(DateTime monthYear)
        {
            var purchases = (from p in _db.Purchases.Where(i => i.PaymentType == PurchaseTypeNames.Normal && i.Date.Month == monthYear.Date.Month && i.Date.Year == monthYear.Year)
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
            return purchases.OrderBy(i => i.PurchaseType.BudgetTypeName).ToList();
        }

        public bool AddUpdatePurchase(Purchase inputPurchase, int purchaseId = -1)
        {
            if (purchaseId == -1)
            {
                return AddPurchase(inputPurchase);
            }
            else
            {
                return UpdatePurchase(inputPurchase, purchaseId);
            }
        }

        public bool AddPurchase(Purchase inputPurchase)
        {
            _db.Purchases.Add(inputPurchase);
            _db.SaveChanges();
            try
            {
                var checkPurchase = _db.Purchases.Where(i => i.Amount == inputPurchase.Amount).FirstOrDefault();
                return true;
            }
            catch
            {
                throw new Exception("Unable to Add Purchase");
            }
        }

        public bool UpdatePurchase(Purchase inputPurchase, int purchaseId)
        {
            try
            {
                var checkPurchase = _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault();
                _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault().Amount = inputPurchase.Amount;
                _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault().Date = inputPurchase.Date;
                _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault().Description = inputPurchase.Description;
                _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault().FutureReimbursement = inputPurchase.FutureReimbursement;
                _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault().GiftCardId = inputPurchase.GiftCardId;
                _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault().PaymentType = inputPurchase.PaymentType;
                _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault().PurchaseTypeId = inputPurchase.PurchaseTypeId;
                _db.SaveChanges();
                return true;
            }
            catch (Exception ee)
            {
                throw new Exception("Unable to update Purchase", ee.InnerException);
            }
        }

        public bool DeletePurchaseEntry(int purchaseId)
        {
            bool success = false;
            try
            {
                var toDelete = _db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault();
                _db.Purchases.Remove(toDelete);
                _db.SaveChanges();
                return success;
            }
            catch
            {
                return success;
            }
        }

        public PurchaseLine GetExistingPurchase(int purchaseId)
        {
            var existingPurchase = (from p in _db.Purchases.Where(i => i.Id == purchaseId)
                                    select new PurchaseLine
                                    {
                                        Amount = p.Amount,
                                        Date = p.Date,
                                        PurchaseType = new BudgetType
                                        {
                                            BudgetTypeId = p.PurchaseTypeId,
                                        },
                                        Description = p.Description,
                                        PaymentType = p.PaymentType,
                                        IsReimbursement = p.FutureReimbursement
                                    }).FirstOrDefault();
            if (existingPurchase.PaymentType == PurchaseTypeNames.GiftCard)
            {
                existingPurchase.GiftCardId = (int)_db.Purchases.Where(i => i.Id == purchaseId).FirstOrDefault().GiftCardId;
            }
            return existingPurchase;
        }
    }
}
