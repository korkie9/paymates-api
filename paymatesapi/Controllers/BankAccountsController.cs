using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using paymatesapi.DTOs;
using paymatesapi.Entities;
using paymatesapi.Models;
using paymatesapi.Services;

namespace paymatesapi.Controllers
// TODO: Run migration
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountsController(IBankAccountService bankAccountService) : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService = bankAccountService;

        [HttpPost("add-bank-account"), Authorize]
        public async Task<ActionResult<BaseResponse<BankAccount>>> AddBankAccount(
            BankAccountDto bankDto
        )
        {
            var bankAccount = await _bankAccountService.AddBankAccount(bankDto);
            if (bankAccount?.Error?.Message != null)
                return BadRequest(bankAccount);
            return Ok(bankDto);
        }

        [HttpDelete("remove-bank-account"), Authorize]
        public async Task<ActionResult<BaseResponse<bool>>> RemoveBankAccount(
            BankAccountDeleteRequest request
        )
        {
            var bankAccountDeleted = await _bankAccountService.DeleteBankAccount(
                request.BankAccountUid
            );
            if (bankAccountDeleted?.Data == false | bankAccountDeleted?.Error?.Message != null)
                return BadRequest(bankAccountDeleted);
            return Ok(bankAccountDeleted);
        }

        [HttpGet("get-bank-accounts"), Authorize]
        public async Task<ActionResult<BaseResponse<List<BankAccount>>>> GetBankAccounts(
            BankAccountsRequest requestData
        )
        {
            var bankAccounts = await _bankAccountService.GetBankAccounts(requestData.UserUid);
            return bankAccounts?.Error?.Message != null
                ? BadRequest(bankAccounts)
                : Ok(bankAccounts);
        }
    }
}
