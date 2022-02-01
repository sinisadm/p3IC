using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Dropbox.Application.Common.Exceptions;
using Dropbox.Application.Common.Interfaces;

namespace Dropbox.Application.UsersDevices.Commands
{
    public class DeleteUserDeviceCommand : IRequest
    {
        public DeleteUserDeviceCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserDeviceCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteUserCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(DeleteUserDeviceCommand command, CancellationToken cancellationToken)
        {

            var dateNow = DateTime.Now;

            var userDevice = await _context.UsersDevices.FirstOrDefaultAsync(t => t.Id == command.Id);

            if (userDevice == null)
            {
                throw new NotFoundException($"Catalog item with Id: {command.Id} does not exist in database!");
            }

            _context.UsersDevices.Remove(userDevice);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
