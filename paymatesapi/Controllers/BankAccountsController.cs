using Microsoft.AspNetCore.Mvc;
using paymatesapi.DTOs;
using Microsoft.AspNetCore.Authorization;
using paymatesapi.Models;
using paymatesapi.Entities;
using paymatesapi.Services;

namespace paymatesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountsController(IBankAccountService bankAccountService) : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService = bankAccountService;

        [HttpPost("add-bank-account"), Authorize]
        public async Task<ActionResult<BaseResponse<BankAccount>>> AddBankAccount(BankAccountDto bankDto)
        {
            var bankAccount = await _bankAccountService.AddBankAccount(bankDto);
            if (bankAccount?.Error?.Message != null) return BadRequest(bankAccount);
            return Ok(bankDto);
        }

        [HttpDelete("remove-bank-account"), Authorize]
        public async Task<ActionResult<BaseResponse<bool>>> RemoveBankAccount(string bankAccountUid)
        {
            var bankAccountDeleted = await _bankAccountService.DeleteBankAccount(bankAccountUid);
            if (bankAccountDeleted?.Data == false | bankAccountDeleted?.Error?.Message != null)
                return BadRequest(bankAccountDeleted);
            return Ok(bankAccountDeleted);
        }

        [HttpGet("get-bank-accounts"), Authorize]
        public async Task<ActionResult<BaseResponse<List<BankAccount>>>> GetBankAccounts(string userId)
        {
            var bankAccounts = await _bankAccountService.GetBankAccounts(userId);
            if (bankAccounts?.Error?.Message != null) return BadRequest(bankAccounts);
            return Ok(bankAccounts);
        }

    }
}
