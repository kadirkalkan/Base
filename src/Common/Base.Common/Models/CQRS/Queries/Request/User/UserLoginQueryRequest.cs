using Base.Common.Models.CQRS.Queries.Response.User;
using MediatR;

namespace Base.Common.Models.CQRS.Queries.Request.User;

public class UserLoginQueryRequest : IRequest<UserLoginQueryResponse>
{
    public string EmailAddress { get; set; }
    public string Password { get; set; }

}
