using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Dropbox.Application.Common;
using Dropbox.Application.Common.Specifications;
using Dropbox.Application.UsersDevices.Queries;
using Dropbox.Application.UsersDevices.Commands;

namespace Dropbox.API.Controllers
{
    [Route("api/userdevice")]
    [ApiController]
    public class UserDeviceController : Controller
    {
        private readonly IMediator _mediator;
        public UserDeviceController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        [ProducesResponseType(typeof(PageableCollection<UserDeviceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers(Guid? userId, Guid? deviceId,[FromQuery] QueryOptions queryOptions)
        {
            var specification = new UserDeviceSpecification(userId, deviceId);
            return Ok(await _mediator.Send(new GetUserDevicesQuery(specification, queryOptions)));
        }

        [HttpGet("{userDeviceId}")]
        [ProducesResponseType(typeof(UserDeviceDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUser(Guid userDeviceId)
        {
            return Ok(await _mediator.Send(new GetUserDeviceQuery(userDeviceId)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateTopic(CreateUserDeviceCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        [HttpDelete("{userDeviceId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteTopic(Guid userDeviceId)
        {
            await _mediator.Send(new DeleteUserDeviceCommand(userDeviceId));
            return NoContent();
        }
    }
}
