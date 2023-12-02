using Base.Api.Application.Interfaces.Repositories;
using Base.Common.Infrastructure;
using Base.Common.Infrastructure.Exceptions;
using Base.Common.Models.CQRS.Commands.Request.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Api.Application.Features.CQRS.Handlers.Commands.User;

public class UserChangePasswordCommandHandler : IRequestHandler<UserChangePasswordCommandRequest, bool>
{
    private readonly IUserRepository _userRepository;

    public UserChangePasswordCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(UserChangePasswordCommandRequest request, CancellationToken cancellationToken)
    {
        if (!request.UserId.HasValue)
            throw new ArgumentNullException(nameof(request.UserId));
        
        var dbUser = await _userRepository.GetByIdAsync(request.UserId.Value);

        if (dbUser is null)
            throw new DatabaseValidationException("User not found!");

        var encPass = PasswordEncryptor.Encrypt(request.OldPassword);
        if (dbUser.Password != encPass)
            throw new DatabaseValidationException("Old password wrong!");
    
        dbUser.Password = encPass;

        await _userRepository.UpdateAsync(dbUser);

        return true;
    }
}
