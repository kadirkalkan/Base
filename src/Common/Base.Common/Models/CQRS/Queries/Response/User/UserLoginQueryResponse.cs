using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Models.CQRS.Queries.Response.User;

public class UserLoginQueryResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }
}
