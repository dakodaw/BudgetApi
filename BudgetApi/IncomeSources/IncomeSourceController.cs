using Budget.Models;
using BudgetApi.Incomes.Models;
using BudgetApi.Incomes.Services;
using BudgetApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BudgetApi.Incomes
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class IncomeSourceController : ControllerBase
    {
        IIncomeSourceService _incomeService;
        public IncomeSourceController(IIncomeSourceService incomeService)
        {
            _incomeService = incomeService;
        }

        [HttpGet]
        [Route("list")]
        public List<IncomeSource> GetIncomeSources()
        {
            return _incomeService.GetIncomeSources();
        }

        //[HttpGet]
        //[Route("fullList")]
        //public List<IncomeSources> GetFullIncomeSources()
        //{
        //    return _incomeService.GetFullIncomeSources();
        //}

        [HttpDelete]
        [Route("{sourceId}")]
        public void DeleteJobEntry(int sourceId)
        {
            _incomeService.DeleteIncomeSource(sourceId);
        }

        [HttpGet]
        [Route("{sourceId}")]
        public IncomeSource GetIncomeSource(int sourceId)
        {
            return _incomeService.GetIncomeSource(sourceId);
        }

        [HttpPut]
        [Route("{sourceId}")] 
        public void UpdateIncomeSource([FromBody] IncomeSource incomeSourceToUpdate, int sourceId)
        {
            _incomeService.UpdateIncomeSource(incomeSourceToUpdate);
        }

        [HttpPost]
        [Route("")]
        public int AddIncomeSource([FromBody] IncomeSource incomeSourceToUpdate)
        {
            return _incomeService.AddIncomeSource(incomeSourceToUpdate);
        }
    }
}
