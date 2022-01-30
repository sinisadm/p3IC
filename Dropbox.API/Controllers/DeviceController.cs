using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Dropbox.Application.Devices.Queries;
using Dropbox.Application.Devices.Commands;
using Dropbox.Application.Common;
using Dropbox.Application.Common.Specifications;

namespace Dropbox.API.Controllers
{
    [Route("api/device")]
    [ApiController]
    public class DeviceController : Controller
    {
        private readonly IMediator _mediator;
        public DeviceController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("{deviceId}")]
        [ProducesResponseType(typeof(DeviceDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDevice(Guid deviceId)
        {
            return Ok(await _mediator.Send(new GetDeviceQuery(deviceId)));
        }

        [HttpGet]
        [ProducesResponseType(typeof(PageableCollection<DeviceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDevices(Guid? deviceId, string macAddress, [FromQuery] QueryOptions queryOptions)
        {
            var specification = new DeviceSpecification(deviceId, macAddress);
            return Ok(await _mediator.Send(new GetDevicesQuery(specification, queryOptions)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateTopic(CreateDeviceCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        [ProducesResponseType(typeof(Unit), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateUser(UpdateDeviceCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{deviceId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteTopic(Guid userId)
        {
            await _mediator.Send(new DeleteDeviceCommand(userId));
            return NoContent();
        }
    }
}
