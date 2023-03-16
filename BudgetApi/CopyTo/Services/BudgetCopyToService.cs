using BudgetApi.Budgeting.Services;
using BudgetApi.CopyTo.Models;
using BudgetApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetApi.CopyTo.Services
{
    public class BudgetCopyToService: IBudgetCopyToService
    {
        private readonly IBudgetService _budgetService;
        public BudgetCopyToService(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        public void CopyFrom(DateTime monthYear, CopyFromRequest request)
        {
            var defaultLastMonth = monthYear.AddMonths(-1);
            var lastMonth = request.FromMethod switch
            {
                CopyFromEnum.PreviousMonth => defaultLastMonth,
                CopyFromEnum.PreviousYear => monthYear.AddYears(-1),
                CopyFromEnum.SpecificMonthDate => request.MonthYear.HasValue ? request.MonthYear.Value : defaultLastMonth,
                _ => defaultLastMonth
            };


            var lastMonthBudgetLines = _budgetService.GetBudgetLines(lastMonth);

            var copiedBudgetLines = new List<BudgetEntry>();
            lastMonthBudgetLines
                .Where(x => 
                    x.BudgetType?.BudgetTypeName != "Totals"
                    && x.BudgetType?.BudgetTypeId != 0)
                .ToList()
                .ForEach(line => 
                {
                    copiedBudgetLines.Add(new BudgetEntry()
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
