using CleanArchitecture.Application.Transactions.Commands;
using CleanArchitecture.Application.Transactions.Commands.CreateTransaction;
using CleanArchitecture.Application.Transactions.Queries.GetCurrentUserTransactionsQuery;
using CleanArchitecture.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    [HttpPost("Create")]
    public async Task<ActionResult<int>> Create(CreateTransactionCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet("GetCurrentUserTransactions")]
    public async Task<List<Transaction>> GetCurrentUserTransactions()
    {
        return await Mediator.Send(new GetCurrentUserTransactionsQuery()); ;
    }
}
