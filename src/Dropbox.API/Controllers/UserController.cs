using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Dropbox.Application.Common;
using Dropbox.Application.Common.Specifications;
using Dropbox.Application.Users.Queries;
using Dropbox.Application.Users.Commands;

namespace Dropbox.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        [ProducesResponseType(typeof(PageableCollection<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers(Guid? id, string email,[FromQuery] QueryOptions queryOptions)
        {
            var specification = new UserSpecification(id, email);
            return Ok(await _mediator.Send(new GetUsersQuery(specification, queryOptions)));
        }
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            return Ok(await _mediator.Send(new GetUserQuery(userId)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateTopic(CreateUserCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        [ProducesResponseType(typeof(Unit), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateUser(UpdateUserCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteTopic(Guid userId)
        {
            await _mediator.Send(new DeleteUserCommand(userId));
            return NoContent();
        }
    }
}
