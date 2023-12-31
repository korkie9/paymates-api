using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using paymatesapi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using paymatesapi.Services;
using paymatesapi.Models;

namespace paymatesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountsController : ControllerBase
    {

        public BankAccountsController()
        {
        }

        [HttpPost("add-bank-account"), Authorize]
        public ActionResult AddBankAccount(BankAccountDto bankDto)
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
