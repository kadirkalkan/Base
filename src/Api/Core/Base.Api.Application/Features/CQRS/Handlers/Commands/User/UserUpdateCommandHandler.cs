using AutoMapper;
using Base.Api.Application.Interfaces.Repositories;
using Base.Api.Domain.Models;
using Base.Common;
using Base.Common.Events.User;
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

public class UserUpdateCommandHandler : IRequestHandler<UserUpdateCommandRequest, Guid>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserUpdateCommandHandler(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }


    public async Task<Guid> Handle(UserUpdateCommandRequest request, CancellationToken cancellationToken)
    {
        var dbUser = await _userRepository.GetByIdAsync(request.Id);

        if (dbUser is null)
            throw new DatabaseValidationException("User not found!");

        var dbEmailAddress = dbUser.EmailAddress;
        var emailChanged = string.CompareOrdinal(dbEmailAddress, request.EmailAddress) != 0;

        // This usage assign all datas into source instead of creating a new instance
        _mapper.Map(request, dbUser);

        var rows = await _userRepository.UpdateAsync(dbUser);

        // Check if email changed

        if (emailChanged && rows > 0)
        {
            var @event = new UserEmailChangedEvent()
            {
                OldEmailAddress = dbEmailAddress,
                NewEmailAddress = request.EmailAddress
            };
            QueueFactory.SendMessageToExchange(exchangeName: BaseConstants.UserExchangeName,
                                               exchangeType: BaseConstants.DefaultExchangeType,
                                               queueName: BaseConstants.UserEmailChangedQueueName,
                                               obj: @event);

            dbUser.EmailConfirmed = false;
            await _userRepository.UpdateAsync(dbUser);
        }

        return dbUser.Id;
    }
}
