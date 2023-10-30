using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using paymatesapi.Models;
using paymatesapi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using paymatesapi.Helpers;
using paymatesapi.Entities;
using paymatesapi.DTOs;

namespace paymatesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("create-transaction"), Authorize]
        public ActionResult<Transaction> CreateTransaction(TransactionDTO transactionDTO)
        {
            Transaction newTransaction = _transactionService.createTransaction(transactionDTO);
            return Ok(newTransaction);
        }

        [HttpPost("get-transactions"), Authorize]
        public ActionResult<Transaction> GetTransactions(TransactionDTO transactionDTO)
        {
            return Ok("hello world");
        }

    }
}
