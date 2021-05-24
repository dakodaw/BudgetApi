using BudgetApi.Incomes.Services;
using BudgetApi.Models;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetApi.Incomes
{
    public class IncomeController : ControllerBase
    {
        IIncomeService _incomeService;
        public IncomeController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        [HttpGet]
        [Route("getIncomeTypes")]
        public List<IncomeSourceLine> GetIncomeTypes()
        {
            return _incomeService.GetIncomeTypes();
        }

        [HttpGet]
        [Route("getIncomeLines")]
        public List<IncomeLine> GetIncomeLines([FromQuery] DateTime monthYear)
        {
            return _incomeService.GetIncomeLines(monthYear);
        }

        [HttpGet]
        [Route("getIncomeSources")]
        public List<IncomeSourceLine> GetIncomeSources()
        {
            return _incomeService.GetIncomeSources();
        }

        [HttpGet]
        [Route("getFullIncomeSources")]
        public List<IncomeSources> GetFullIncomeSources()
        {
            return _incomeService.GetFullIncomeSources();
        }

        [HttpGet]
        [Route("getApplicablePurchases")]
        public List<ApplicablePurchase> GetApplicablePurchases([FromQuery] DateTime monthYear)
        {
            return _incomeService.GetApplicablePurchases(monthYear);
        }

        [HttpPost]
        [Route("addUpdateIncome")]
        public bool AddUpdateIncome([FromBody] Income inputIncome, [FromQuery] int incomeId = -1)
        {
            return _incomeService.AddUpdateIncome(inputIncome, incomeId);
        }

        [HttpGet]
        [Route("deleteIncomeEntry")]
        public bool DeleteIncomeEntry([FromQuery] int incomeId)
        {
            return _incomeService.DeleteIncomeEntry(incomeId);
        }

        [HttpPost]
        [Route("addUpdateJob")]
        public bool AddUpdateJob([FromBody] IncomeSource inputJob, [FromQuery] int incomeSourceId = -1)
        {
            return _incomeService.AddUpdateJob(inputJob, incomeSourceId);
        }

        [HttpGet]
        [Route("deleteJobEntry")]
        public bool DeleteJobEntry([FromQuery] int incomeSourceId)
        {
            return _incomeService.DeleteJobEntry(incomeSourceId);
        }

        [HttpGet]
        [Route("getIncomeSource")]
        public IncomeSources GetIncomeSource([FromQuery] int incomeSourceId)
        {
            return _incomeService.GetIncomeSource(incomeSourceId);
        }

        [HttpGet]
        [Route("getExistingIncome")]
        public IncomeLine GetExistingIncome([FromQuery] int incomeId)
        {
            return _incomeService.GetExistingIncome(incomeId);
        }
    }
}
