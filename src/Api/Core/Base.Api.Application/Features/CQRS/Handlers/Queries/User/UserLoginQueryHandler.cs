using AutoMapper;
using Base.Api.Application.Interfaces.Repositories;
using Base.Common.Infrastructure;
using Base.Common.Infrastructure.Exceptions;
using Base.Common.Models.CQRS.Queries.Request.User;
using Base.Common.Models.CQRS.Queries.Response.User;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Base.Api.Application.Features.CQRS.Handlers.Queries.User;

public class UserLoginQueryHandler : IRequestHandler<UserLoginQueryRequest, UserLoginQueryResponse>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public UserLoginQueryHandler(IUserRepository repository, IMapper mapper, IConfiguration configuration)
    {
        _repository = repository;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<UserLoginQueryResponse> Handle(UserLoginQueryRequest request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetSingleAsync(x => x.EmailAddress.Equals(request.EmailAddress));

        if (user == null)
            throw new DatabaseValidationException("User not found!");

        var password = PasswordEncryptor.Encrypt(request.Password);

        if (user.Password != password)
            throw new DatabaseValidationException("Password is wrong!");

        if (!user.EmailConfirmed)
            throw new DatabaseValidationException("Email address is not confirmed yet!");

        var result = _mapper.Map<UserLoginQueryResponse>(user);

        var claims = new Claim[] { 
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.EmailAddress),
            new Claim(ClaimTypes.Name, user.Username),
        };

        result.Token = GenerateToken(claims);

        return result;
    }

    private string GenerateToken(Claim[] claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthConfig:Secret"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.Now.AddDays(10);

        var token = new JwtSecurityToken(claims: claims,
                                         expires: expiry,
                                         signingCredentials: credentials,
                                         notBefore: DateTime.Now);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
