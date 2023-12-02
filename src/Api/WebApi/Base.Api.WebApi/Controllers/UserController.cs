using Base.Common.Infrastructure;
using Base.Common.Models.CQRS.Queries.Request.User;
using Base.Common.Models.CQRS.Commands.Request.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Base.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginQueryRequest request)
        {
            var res = await _mediator.Send(request);
            
            return Ok(res);
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Create([FromBody] UserCreateCommandRequest request)
        {
            var guid = await _mediator.Send(request);

            return Ok(guid);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateCommandRequest request)
        {
            var guid = await _mediator.Send(request);

            return Ok(guid);
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordCommandRequest request)
        {
            var guid = await _mediator.Send(request);

            return Ok(guid);
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] UserConfirmEmailCommandRequest request)
        {
            var guid = await _mediator.Send(request);

            return Ok(guid);
        }



        // todo This Endpoint for test purposes. It'll be deleted.
        [HttpGet]
        [Route("GetHashPassword/{password}")]
        public IActionResult GetMD5(string password)
        {
            return Ok(PasswordEncryptor.Encrypt(password));
        }
    }
}
