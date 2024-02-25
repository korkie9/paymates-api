using Microsoft.AspNetCore.Mvc;
using paymatesapi.DTOs;
using Microsoft.AspNetCore.Authorization;
using paymatesapi.Models;
using paymatesapi.Entities;

namespace paymatesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountsController() : ControllerBase
    {

        [HttpPost("add-bank-account"), Authorize]
        public ActionResult<BaseResponse<BankAccount>> AddBankAccount(BankAccountDto bankDto)
        {
            
            return Ok(bankDto);
        }

        [HttpDelete("remove-bank-account"), Authorize]
        public ActionResult RemoveBankAccount(BankAccountRequest bankAccountRequest)
        {
            return Ok(bankAccountRequest);
        }

        [HttpGet("get-bank-accounts"), Authorize]
        public ActionResult GetBankAccounts(UserRequest userRequest)
        {
            return Ok();
        }

    }
}
