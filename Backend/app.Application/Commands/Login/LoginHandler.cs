using MediatR;
using app.Application.Interfaces;
using app.Application.DTOs;

namespace app.Application.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, AuthenticationResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public LoginHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<AuthenticationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            var user = await _userRepository.GetByUsernameAsync(request.Username);


            if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            {

                throw new UnauthorizedAccessException("Invalid username or password");
            }

            string token = _jwtProvider.Generate(user);

            return new AuthenticationResult(token);
        }
    }
}