using BudgetApi.Incomes.Models;
using BudgetApi.Incomes.Services;
using BudgetApi.Models;
using BudgetApi.Purchases.Models;
using BudgetApi.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BudgetApi.Incomes
{
    [ApiController]
    [Route("[controller]")]
    public class IncomeController : ControllerBase
    {
        IIncomeService _incomeService;
        public IncomeController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        [HttpPost]
        [Route("")]
        public int AddIncome([FromBody] Income inputIncome)
        {
            return _incomeService.AddIncome(inputIncome);
        }

        [HttpPut]
        [Route("{incomeId}")]
        public bool UpdateIncome([FromBody] Income inputIncome, int incomeId = -1)
        {
            // TODO: Handle Not found and incomeId of less than 1 passed through
            return _incomeService.UpdateIncome(inputIncome, incomeId);
        }

        [HttpDelete]
        [Route("{incomeId}")]
        public bool DeleteIncome(int incomeId = -1)
        {
            // TODO: Handle Not found and incomeId of less than 1 passed through
            return _incomeService.DeleteIncomeEntry(incomeId);
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
        public List<IncomeSource> GetFullIncomeSources()
        {
            return _incomeService.GetFullIncomeSources();
        }

        [HttpGet]
        [Route("getApplicablePurchases")]
        public List<ApplicablePurchase> GetApplicablePurchases([FromQuery] DateTime monthYear)
        {
            return _incomeService.GetApplicablePurchases(monthYear);
        }

        [Obsolete("The base route using post, and put will be used moving forward")]
        [HttpPost]
        [Route("addUpdateIncome")]
        public bool AddUpdateIncome([FromBody] Income inputIncome, [FromQuery] int incomeId = -1)
        {
            return _incomeService.AddUpdateIncome(inputIncome, incomeId);
        }

        [Obsolete("The base route using delete will be used moving forward")]
        [HttpGet]
        [Route("deleteIncomeEntry")]
        public bool DeleteIncomeEntry([FromQuery] int incomeId)
        {
            return _incomeService.DeleteIncomeEntry(incomeId);
        }

        [HttpPost]
        [Route("addUpdateJob")]
        public bool AddUpdateJob([FromBody] IncomeSourceEntity inputJob, [FromQuery] int incomeSourceId = -1)
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
        public IncomeSource GetIncomeSource([FromQuery] int incomeSourceId)
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
