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
        private readonly IJwtUtils _jwtUtils;
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService, IJwtUtils jwtUtils)
        {
            _jwtUtils = jwtUtils;
            _transactionService = transactionService;
        }

        [HttpPost("create-transaction"), Authorize]
        public async Task<IActionResult> CreateTransaction(TransactionDTO transactionDTO)
        {
            var userId = _jwtUtils.GetUidFromHeaders();
            if (String.IsNullOrEmpty(userId)) return Unauthorized(new { message = "User is not authenticated" });
            if (transactionDTO.FriendUid == null) return BadRequest(new { messge = "Friend ID is required" });

            Transaction newTransaction = await _transactionService.createTransaction(userId, transactionDTO);
            return Ok(newTransaction);
        }

        [HttpPost("get-transactions"), Authorize]
        public ActionResult<ICollection<Transaction>> GetTransactions(string friendUid)
        {
            var userId = _jwtUtils.GetUidFromHeaders();
            if (String.IsNullOrEmpty(userId)) return Unauthorized(new { message = "User is not authenticated" });

            var transactions = _transactionService.getTransactions(userId, friendUid);
            if (transactions == null) return NotFound(new { message = "Transactions not found" });
            return Ok(transactions);
        }

        [HttpPost("get-transaction"), Authorize]
        public ActionResult<Transaction> GetTransaction(string transactionUid)
        {
            var transaction = _transactionService.getTransaction(transactionUid);
            if (transaction == null) return NotFound(new { message = "Transaction not found" });
            return Ok(transaction);
        }

    }
}
