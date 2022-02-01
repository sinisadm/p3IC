using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Dropbox.Application.Common.Exceptions;
using Dropbox.Application.Common.Interfaces;

namespace Dropbox.Application.Devices.Commands
{
    public class UpdateDeviceCommand : IRequest
    {
        public Guid Id { get; set; }
        public string MacAddress { get; set; }
        public string Name { get; set; }
    }

    public class UpdateDeviceCommandValidator : AbstractValidator<UpdateDeviceCommand>
    {
        public UpdateDeviceCommandValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
        }
    }

    public class UpdateDeviceCommandHandler : IRequestHandler<UpdateDeviceCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateDeviceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(UpdateDeviceCommand command, CancellationToken cancellationToken)
        {
            var device = await _context.Devices
                .FirstOrDefaultAsync(t => t.Id == command.Id);

            if (device == null)
            {
                throw new NotFoundException($"Cannot find CatalogItem with Id: {command.Id}");
            }

            if (!string.IsNullOrEmpty(command.MacAddress.Trim()))
                device.MacAddress = command.MacAddress;

            if (!string.IsNullOrEmpty(command.Name.Trim()))
                device.Name = command.Name;


            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
