using Base.Api.Application.Interfaces.Repositories;
using Base.Api.Domain.Models;
using Base.Infrastructure.Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Infrastructure.Persistance.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(BaseContext context) : base(context)
    {
    }
}
