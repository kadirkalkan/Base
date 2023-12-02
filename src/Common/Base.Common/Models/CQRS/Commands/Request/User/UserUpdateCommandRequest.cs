using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Models.CQRS.Commands.Request.User;

public class UserUpdateCommandRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string EmailAddress { get; set; }
}
