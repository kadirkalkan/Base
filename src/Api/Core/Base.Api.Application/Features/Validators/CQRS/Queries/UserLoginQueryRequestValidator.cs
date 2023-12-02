using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Common.Models.CQRS.Queries.Request.User;

namespace Base.Api.Application.Features.Validators.CQRS.Queries;

public class UserLoginQueryRequestValidator : AbstractValidator<UserLoginQueryRequest>
{
    public UserLoginQueryRequestValidator()
    {
        RuleFor(x => x.EmailAddress)
            .NotNull()
            .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
            .WithMessage("{PropertyName} not a valid email address");

        RuleFor(x => x.Password)
            .NotNull()
            .MinimumLength(6)
            .WithMessage("{PropertyName} should at least be {MinLength} characters");
    }
}
