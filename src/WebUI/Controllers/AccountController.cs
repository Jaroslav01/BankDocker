using CleanArchitecture.Application.Accounts.Commands;
using CleanArchitecture.Application.Accounts.Commands.CreateAccount;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebUI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    [HttpPost("Create")]
    public async Task<ActionResult<int>> Create(CreateAccountCommand command)
    {
        return await Mediator.Send(command);
    }
}
