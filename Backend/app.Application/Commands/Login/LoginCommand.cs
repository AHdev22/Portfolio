using app.Application.DTOs;
using MediatR;

namespace app.Application.Commands.Login
{
    public sealed record LoginCommand(string Username, string Password) : IRequest<AuthenticationResult>;
}


