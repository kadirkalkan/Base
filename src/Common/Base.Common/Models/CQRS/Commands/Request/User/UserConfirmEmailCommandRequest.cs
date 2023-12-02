using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Models.CQRS.Commands.Request.User;

public class UserConfirmEmailCommandRequest : IRequest<bool>
{
    public Guid ConfirmationId { get; set; }
}
