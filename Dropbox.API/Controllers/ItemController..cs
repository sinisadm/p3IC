using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Dropbox.Application.Common;
using Dropbox.Application.Common.Specifications;
using Dropbox.Application.Items.Queries;
using Dropbox.Application.Items.Commands;

namespace Dropbox.API.Controllers
{
    [Route("api/item")]
    [ApiController]
    public class ItemController : Controller
    {
        private readonly IMediator _mediator;
        public ItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PageableCollection<ItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetItems(Guid userDeviceId, Guid? parentItemId, string name, [FromQuery] QueryOptions queryOptions)
        {
            var specification = new ItemSpecification( userDeviceId, parentItemId, name);
            return Ok(await _mediator.Send(new GetItemsQuery(specification, queryOptions)));
        }
        [HttpGet("{itemId}")]
        [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetItem(Guid itemId)
        {
            return Ok(await _mediator.Send(new GetItemQuery(itemId)));
        }

        [HttpPost("file")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateFile(CreateFileCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPost("folder")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateFolder(CreateFolderCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        [ProducesResponseType(typeof(Unit), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateUser(UpdateItemCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{itemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteTopic(Guid itemId)
        {
            await _mediator.Send(new DeleteItemCommand(itemId));
            return NoContent();
        }
    }
}
