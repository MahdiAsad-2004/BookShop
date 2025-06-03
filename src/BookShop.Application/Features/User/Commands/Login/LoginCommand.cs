using MediatR;
using BookShop.Domain.Common;
using BookShop.Application.Common.Request;
using BookShop.Domain.IRepositories;
using BookShop.Domain.Identity;
using BookShop.Domain.Enums;

namespace BookShop.Application.Features.User.Commands.Login
{
    public class LoginCommand : IRequest<Result<LoginCommandResponse>> , IValidatableRquest
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
    {
        #region constructor

        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        public LoginCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        #endregion

        public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.Login(request.Username, _passwordHasher.Hash(request.Password));
            if (result != null)
                return new Result<LoginCommandResponse>
                {
                    Data = new LoginCommandResponse(result.Value.id, request.Username, result.Value.role),
                    IsSuccess = true,
                };
            
            return new Result<LoginCommandResponse>(null, false, error: new Error(ErrorCode.Invalid_Credentials , "Incorrect username or password"));
        }
    }

}
