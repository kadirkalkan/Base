using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Models.CQRS.Commands.Request.User;

public class UserChangePasswordCommandRequest :IRequest<bool>
{
    public Guid? UserId { get; set; }

    public string OldPassword { get; set; }

    public string NewPassword { get; set; }

}
