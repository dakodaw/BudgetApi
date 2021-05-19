using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetTest.Controllers;
using BudgetTest.Models;

namespace BudgetTest.Models
{

    public static class StringExtension
    {
        public static string GetLast(this string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }
    }

    public class GiftCardHistoryBalance
    {
        public string Place { get; set; }
        public string CardNo { get; set; }
        public string AccessCode { get; set; }
        public List<Purchase> History { get; set; }
        public decimal Balance { get; set; }
    }

    public class GiftCardSelectLine
    {
        public int Id { get; set; }
        public string Place { get; set; }
        public decimal RemainingAmount { get; set; }
        public string Last4ofCardNumber { get; set; }
    }

    public class ApplicablePurchase
    {
        public int Id { get; set; }
        public string PurchaseType { get; set; }
        public decimal Amount { get; set; }
    }
    public class IncomeSources : IncomeSourceLine
    {
        public decimal EstimatedIncome { get; set; }
        public string PayFrequency { get; set; }
    }
    public class ScenarioInput
    {
        public double initialAmount { get; set; }
        public DateTime startMonth { get; set; }
        public DateTime endMonth { get; set; }
    }

    public class BudgetTypes
    {
        public int BudgetTypeId { get; set; }
        public string BudgetTypeName { get; set; }
    }
    public class BudgetInfo
    {
        public int BudgetLineId { get; set; }
        public BudgetTypes BudgetType { get; set; }
        public DateTime BudgetDate { get; set; }
        public string BudgetMonthYear { get; set; }
        public decimal Amount { get; set; }
    }

    public class BudgetWithPurchaseInfo : BudgetInfo
    {
        public decimal PurchaseAmount { get; set; }
    }

    public class IncomeSourceLine
    {
        public int IncomeSourceId { get; set; }
        public string IncomeSource { get; set; }
        public string JobOf { get; set; }
        public string Position { get; set; }
        public bool IsCurrentJob { get; set; }
    }

    public class ShortIncomeLine
    {
        public int IncomeId { get; set; }
        public int IncomeSourceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime IncomeDate { get; set; }
    }
    public class IncomeLine : ShortIncomeLine
    {
        public IncomeSourceLine IncomeSource { get; set; }
        public string Details { get; set; }
        public bool IsReimbursement { get; set; }
        public int PurchaseId { get; set; }
        public bool IsCash { get; set; }
    }
    public class PurchaseLine
    {
        public int Id { get; set; }
        public BudgetTypes PurchaseType { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsReimbursement { get; set; }
        public string PaymentType { get; set; }
        public string Description { get; set; }
        public int GiftCardId { get; set; }
    }

    public static class PurchaseTypeNames
    {
        public static string GiftCard = "Gift Card";
        public static string Normal = "Normal";
        public static string Cash = "Cash";
    }

    public static class PaymentFrequency
    {
        public static string Biweekly = "Biweekly";
        public static string Monthly = "Monthly";
        public static string TwiceAMonth = "TwiceAMonth";
    }

    public static class BudgetTypeStatics
    {
        public static string Totals = "Totals";
    }
}