using AutoMapper;
using Base.Api.Domain.Models;
using Base.Common.Models.CQRS.Commands.Request.User;
using Base.Common.Models.CQRS.Queries.Response.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Api.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapping works left to right. Reverse makes it two way
        CreateMap<User, UserLoginQueryResponse>()
            .ReverseMap();

        // User doesn't need to be mapped to UserCreateCommandRequest so One way is enough.
        CreateMap<UserCreateCommandRequest, User>();
        
        CreateMap<UserUpdateCommandRequest, User>();

    }
}
