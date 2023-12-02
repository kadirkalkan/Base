using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Models.CQRS.Commands.Request.User;

public class UserCreateCommandRequest : IRequest<Guid>
{
    public string Username { get; set; }
    public string EmailAddress { get; set; }
    public string Password { get; set; }

}
