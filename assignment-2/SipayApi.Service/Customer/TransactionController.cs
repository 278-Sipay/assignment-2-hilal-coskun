using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SipayApi.Base;
using SipayApi.Data.Domain;
using SipayApi.Data.Repository;
using SipayApi.Schema;
using System.Linq.Expressions;

namespace SipayApi.Service;



[ApiController]
[Route("sipy/api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionRepository repository;
    private readonly IMapper mapper;
    public TransactionController(ITransactionRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }


	[HttpGet("GetByParameter")]
	public ActionResult<IEnumerable<Transaction>> GetByParameter(
		int? accountNumber,
		string referenceNumber,
		decimal? minAmountCredit,
		decimal? maxAmountCredit,
		decimal? minAmountDebit,
		decimal? maxAmountDebit,
		string description,
		DateTime? beginDate,
		DateTime? endDate)
	{
		Expression<Func<Transaction, bool>> expression = transaction =>
			(!accountNumber.HasValue || transaction.AccountNumber == accountNumber.Value)
			&& (string.IsNullOrEmpty(referenceNumber) || transaction.ReferenceNumber == referenceNumber)
			&& (!minAmountCredit.HasValue || transaction.CreditAmount >= minAmountCredit.Value)
			&& (!maxAmountCredit.HasValue || transaction.CreditAmount <= maxAmountCredit.Value)
			&& (!minAmountDebit.HasValue || transaction.DebitAmount >= minAmountDebit.Value)
			&& (!maxAmountDebit.HasValue || transaction.DebitAmount <= maxAmountDebit.Value)
			&& (string.IsNullOrEmpty(description) || transaction.Description == description)
			&& (!beginDate.HasValue || transaction.TransactionDate >= beginDate.Value)
			&& (!endDate.HasValue || transaction.TransactionDate <= endDate.Value);


		IEnumerable<Transaction> filterTransaction = repository.Where(expression);

		return Ok(filterTransaction);
	}

}
