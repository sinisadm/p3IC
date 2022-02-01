using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dropbox.Application.Common.Exceptions;
using Dropbox.Application.Common.Interfaces;
using Dropbox.Domain.Entities;

namespace Dropbox.Application.Devices.Commands
{
    public class CreateDeviceCommand : IRequest<Guid>
    {
        public string MacAddress { get; set; }
        public string Name { get; set; }
    }

    public class CreateDeviceCommandValidator : AbstractValidator<CreateDeviceCommand>
    {
        public CreateDeviceCommandValidator()
        {
            RuleFor(t => t.MacAddress).NotEmpty();
            RuleFor(t => t.Name).NotEmpty();
        }
    }

    public class CreateDeviceCommandHandler : IRequestHandler<CreateDeviceCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateDeviceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateDeviceCommand command, CancellationToken cancellationToken)
        {
            try
            {

                var deviceExists = await _context.Devices.Where(t => t.MacAddress == command.MacAddress).FirstOrDefaultAsync();

                if (deviceExists != null)
                {
                    throw new DuplicateItemException($"Device with Mac Address {command.MacAddress} already exists!");
                }

                var device = new Device
                {
                    Id = Guid.NewGuid(),
                    MacAddress = command.MacAddress,
                    Name = command.Name
                };

                _context.Devices.Add(device);

                await _context.SaveChangesAsync(cancellationToken);

                return device.Id;

            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
