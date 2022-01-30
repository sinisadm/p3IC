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

namespace Dropbox.Application.UsersDevices.Commands
{
    public class CreateUserDeviceCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public Guid DeviceId { get; set; }
        public string SyncFolder { get; set; }
        public string RootFolder { get; set; }
    }

    public class CreateUserDeviceCommandValidator : AbstractValidator<CreateUserDeviceCommand>
    {
        public CreateUserDeviceCommandValidator()
        {
            RuleFor(t => t.UserId).NotEmpty();
            RuleFor(t => t.DeviceId).NotEmpty();
            RuleFor(t => t.SyncFolder).NotEmpty();
            RuleFor(t => t.RootFolder).NotEmpty();
        }
    }

    public class CreateUserDeviceCommandHandler : IRequestHandler<CreateUserDeviceCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateUserDeviceCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateUserDeviceCommand command, CancellationToken cancellationToken)
        {
            try
            {

                var device = await _context.Devices.Where(t => t.Id == command.DeviceId).FirstOrDefaultAsync();

                if (device == null)
                {
                    throw new NotFoundException($"Device with Id {command.DeviceId} not exists!");
                }

                var user = await _context.Users.Where(t => t.Id == command.UserId).FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new NotFoundException($"User with Id {command.UserId} not exists!");
                }

                var userDevice = new UserDevice
                {
                    Id = Guid.NewGuid(),
                    Device = device,
                    User = user,
                    SyncFolder = command.SyncFolder,
                    RootFolder = null
                };

                _context.UsersDevices.Add(userDevice);

                await _context.SaveChangesAsync(cancellationToken);

                Item item = new Item
                {
                    Id = Guid.NewGuid(),
                    UserDevice = userDevice,
                    ItemName = command.RootFolder,
                    CreatedAt = DateTime.Now,
                    IsFolder = true
                };
                _context.Items.Add(item);

                await _context.SaveChangesAsync(cancellationToken);

                userDevice.RootFolder = item;

                await _context.SaveChangesAsync(cancellationToken);

                return userDevice.Id;

            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
