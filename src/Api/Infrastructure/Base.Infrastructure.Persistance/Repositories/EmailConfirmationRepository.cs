using Base.Api.Application.Interfaces.Repositories;
using Base.Api.Domain.Models;
using Base.Infrastructure.Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Infrastructure.Persistance.Repositories;

public class EmailConfirmationRepository : GenericRepository<EmailConfirmation>, IEmailConfirmationRepository
{
    public EmailConfirmationRepository(BaseContext dbContext) : base(dbContext)
    {
    }
}
