using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dropbox.Application.Common.Exceptions;
using Dropbox.Application.Common.Interfaces;

namespace Dropbox.Application.Devices.Commands
{
    public class DeleteDeviceCommand : IRequest
    {
        public DeleteDeviceCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

    public class DeleteDeviceCommandHandler : IRequestHandler<DeleteDeviceCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteDeviceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(DeleteDeviceCommand command, CancellationToken cancellationToken)
        {

            var dateNow = DateTime.Now;

            var device = await _context.Devices.FirstOrDefaultAsync(t => t.Id == command.Id);

            if (device == null)
            {
                throw new NotFoundException($"Catalog item with Id: {command.Id} does not exist in database!");
            }

            device.IsDeleted = true;
            device.DeletedAt = dateNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
