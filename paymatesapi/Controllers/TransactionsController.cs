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
        /* private readonly IJwtUtils _jwtUtils = jwtUtils; */

        private readonly ITransactionService _transactionService = transactionService;

        [HttpPost("create-transaction"), Authorize]
        public async Task<IActionResult> CreateTransaction(TransactionDTO transactionDTO)
        {
            if (transactionDTO.DebtorUsernames == null)
            {
                return BadRequest(
                    new BaseResponse<bool>
                    {
                        Error = new Error { Message = "Debtor username is required" }
                    }
                );
            }
            if (transactionDTO.CreditorUsernames == null)
            {
                return BadRequest(
                    new BaseResponse<bool>
                    {
                        Error = new Error { Message = "Creditor username is required" }
                    }
                );
            }

            BaseResponse<bool> newTransaction = await _transactionService.CreateTransactions(
                transactionDTO
            );
            if (newTransaction.Error != null)
            {
                BaseResponse<bool> errResponse = new BaseResponse<bool>
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
