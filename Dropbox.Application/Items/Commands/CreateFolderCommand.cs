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
    public class CreateFolderCommand : IRequest<Guid>
    {
        public Guid ParentItemId { get; set; }
        public Guid UserDeviceId { get; set; }
        public string ItemName { get; set; }
    }

    public class CreateFolderCommandValidator : AbstractValidator<CreateFolderCommand>
    {
        public CreateFolderCommandValidator()
        {
            RuleFor(t => t.UserDeviceId).NotEmpty();
            RuleFor(t => t.ItemName).NotEmpty();
        }
    }

    public class CreateFolderCommandHandler : IRequestHandler<CreateFolderCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateFolderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateFolderCommand command, CancellationToken cancellationToken)
        {
            try
            {
                UserDevice ud = await _context.UsersDevices.Where(x => x.Id == command.UserDeviceId).FirstOrDefaultAsync();
                if(ud == null)
                {
                    throw new NotFoundException(string.Format("UserDevice with id {0} not exists", command.UserDeviceId));
                }

                var item = new Item
                {
                    Id = Guid.NewGuid(),
                    UserDevice = ud,
                    ItemName = command.ItemName,
                    IsFolder = true,
                    CreatedAt = DateTime.Now
                };

                Item pf = await _context.Items.Where(x => x.Id == command.ParentItemId).FirstOrDefaultAsync();
                if(pf == null)
                {
                    throw new NotFoundException(string.Format("Folder with id {0} not exists", command.ParentItemId));
                }

                if(pf.UserDeviceId != ud.Id)
                {
                    throw new NotFoundException(string.Format("Folder do not belong to UserDevice with an id {0}", command.UserDeviceId));
                }
                item.ParentItem = pf;


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
