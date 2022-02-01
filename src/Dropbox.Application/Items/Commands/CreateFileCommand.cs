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

namespace Dropbox.Application.Items.Commands
{
    public class CreateFileCommand : IRequest<Guid>
    {
        public Guid UserDeviceId { get; set; }
        public Guid ParentItemId { get; set; }
        public string ItemName { get; set; }
        //public bool? IsFolder { get; set; }
        //public int Version { get; set; }
        //public string ItemExtension { get; set; }
        //public string ItemPath { get; set; }
        //public int ItemSize { get; set; }
    }

    public class CreateFileCommandValidator : AbstractValidator<CreateFileCommand>
    {
        public CreateFileCommandValidator()
        {
            RuleFor(t => t.ParentItemId).NotEmpty();
            RuleFor(t => t.UserDeviceId).NotEmpty();
            RuleFor(t => t.ItemName).NotEmpty();
            //RuleFor(t => t.ItemExtension).NotEmpty();
            //RuleFor(t => t.ItemPath).NotEmpty();
            //RuleFor(t => t.ItemSize).NotEmpty();
        }
    }

    public class CreateFileCommandHandler : IRequestHandler<CreateFileCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateFileCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateFileCommand command, CancellationToken cancellationToken)
        {
            try
            {
                UserDevice ud = await _context.UsersDevices
                    .Where(x => x.Id == command.UserDeviceId)
                    .FirstOrDefaultAsync();
                if (ud == null)
                {
                    throw new NotFoundException(string.Format("UserDevice with id {0} not exists!", command.UserDeviceId));
                }

                Item pf = await _context.Items
                    .Where(x => x.Id == command.ParentItemId && x.UserDeviceId == command.UserDeviceId).FirstOrDefaultAsync();
                
                if(pf == null)
                {
                    throw new NotFoundException(string.Format("Folder with id {0} not exists!", command.ParentItemId));
                }

                if (pf.UserDeviceId != ud.Id)
                {
                    throw new NotFoundException(string.Format("Folder do not belong to UserDevice with an ID {0}!", command.UserDeviceId));
                }

                Item item = new()
                {
                    Id = Guid.NewGuid(),
                    UserDeviceId = command.UserDeviceId,
                    ParentItem = pf,
                    ItemName = command.ItemName,
                    //ItemExtension = command.ItemExtension,
                    //ItemSize = command.ItemSize,
                    //ItemPath = command.ItemPath,
                    //Version = 1,
                    IsFolder = false,
                    CreatedAt = DateTime.Now
                };

                _context.Items.Add(item);

                await _context.SaveChangesAsync(cancellationToken);

                return item.Id;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
