using BudgetApi.Budgeting.Services;
using BudgetApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApi.Copy.Services
{
    public class BudgetCopyService: IBudgetCopyService
    {
        private readonly IBudgetService _budgetService;
        public BudgetCopyService(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        public void CopyBudgetFromPreviousMonth(DateTime monthYear)
        {
            var lastMonth = monthYear.AddMonths(-1);
            var lastMonthBudgetLines = _budgetService.GetBudgetLines(lastMonth);

            var copiedBudgetLines = new List<Budget>();
            lastMonthBudgetLines
                .Where(x => 
                    x.BudgetType?.BudgetTypeName != "Totals"
                    && x.BudgetType?.BudgetTypeId != 0)
                .ToList()
                .ForEach(line => 
                {
                    copiedBudgetLines.Add(new Budget()
                    {
                        BudgetTypeId = line.BudgetType.BudgetTypeId,
                        Date = monthYear,
                        Amount = line.Amount
                    });
                });

            _budgetService.AddBudgetLines(copiedBudgetLines);
        }
    }
}
