﻿using CleanArchitecture.Application.Transactions.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers;
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
}