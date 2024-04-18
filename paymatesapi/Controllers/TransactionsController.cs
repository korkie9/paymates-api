using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using paymatesapi.DTOs;
using paymatesapi.Entities;
using paymatesapi.Helpers;
using paymatesapi.Models;
using paymatesapi.Services;

namespace paymatesapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController(ITransactionService transactionService, IJwtUtils jwtUtils)
        : ControllerBase
    {
        private readonly IJwtUtils _jwtUtils = jwtUtils;

        private readonly ITransactionService _transactionService = transactionService;

        [HttpPost("create-transaction"), Authorize]
        public async Task<IActionResult> CreateTransaction(TransactionDTO transactionDTO)
        {
            if (transactionDTO.DebtorUsername == null)
            {
                return BadRequest(
                    new BaseResponse<Transaction>
                    {
                        Error = new Error { Message = "Debtor email is required" }
                    }
                );
            }
            if (transactionDTO.CreditorUsername == null)
            {
                return BadRequest(
                    new BaseResponse<Transaction>
                    {
                        Error = new Error { Message = "Creditor email is required" }
                    }
                );
            }

            BaseResponse<Transaction> newTransaction = await _transactionService.CreateTransaction(
                transactionDTO
            );
            if (newTransaction.Error != null)
            {
                var errResponse = new BaseResponse<Transaction>
                {
                    Error = new Error { Message = newTransaction.Error.Message }
                };
                return BadRequest(errResponse);
            }
            return Ok(newTransaction);
        }

        [HttpPost("get-transactions"), Authorize]
        public ActionResult<BaseResponse<ICollection<Transaction>>> GetTransactions(
            GetTransactionsRequest friendRequest
        )
        {
            BaseResponse<ICollection<Transaction>> transactions =
                _transactionService.GetTransactions(
                    friendRequest.Username,
                    friendRequest.FriendUsername
                );
            return transactions == null
                ? NotFound(new { message = "Transactions not found" })
                : Ok(transactions);
        }

        [HttpPost("get-transaction"), Authorize]
        public ActionResult<Transaction> GetTransaction(TransactionRequest transactionRequest)
        {
            var transaction = _transactionService.GetTransaction(transactionRequest.TransactionUid);
            return transaction == null
                ? NotFound(new { message = "Transaction not found" })
                : Ok(transaction);
        }

        [HttpDelete("delete-transaction"), Authorize]
        public async Task<ActionResult<BaseResponse<bool>>> DeleteTransaction(
            TransactionRequest transactionRequest
        )
        {
            var transaction = await _transactionService.DeleteTransaction(
                transactionRequest.TransactionUid
            );
            return transaction.Data == false ? NotFound(transaction) : Ok(transaction);
        }
    }
}
