using AutoMapper;
using Base.Api.Application.Interfaces.Repositories;
using Base.Common;
using Base.Common.Events.User;
using Base.Common.Infrastructure;
using Base.Common.Infrastructure.Exceptions;
using Base.Common.Models.CQRS.Commands.Request.User;
using MediatR;
using System.Dynamic;

namespace Base.Api.Application.Features.CQRS.Handlers.Commands.User;

public class UserCreateCommandHandler : IRequestHandler<UserCreateCommandRequest, Guid>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserCreateCommandHandler(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(UserCreateCommandRequest request, CancellationToken cancellationToken)
    {
        var existsUser = await _userRepository.GetSingleAsync(x => x.EmailAddress == request.EmailAddress);
        if (existsUser is not null)
            throw new DatabaseValidationException("User already exists!");

        var dbUser = _mapper.Map<Domain.Models.User>(request);

        dbUser.Password = PasswordEncryptor.Encrypt(request.Password);

        var rows = await _userRepository.AddAsync(dbUser);

        // Email Changed/Created

        if (rows > 0)
        {
            //var @event = new UserEmailChangedEvent()
            //{
            //    OldEmailAddress = null,
            //    NewEmailAddress = request.EmailAddress
            //};
            //QueueFactory.SendMessageToExchange(exchangeName: BaseConstants.UserExchangeName,
            //                                   exchangeType: BaseConstants.DefaultExchangeType,
            //                                   queueName: BaseConstants.UserEmailChangedQueueName,
            //                                   obj: @event);
        }

        return dbUser.Id;
    }
}
