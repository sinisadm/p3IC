using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dropbox.Application.Common.Exceptions;
using Dropbox.Application.Common.Interfaces;
using System.Collections.Generic;
using Dropbox.Domain.Entities;

namespace Dropbox.Application.Items.Commands
{
    public class DeleteItemCommand : IRequest
    {
        public DeleteItemCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(DeleteItemCommand command, CancellationToken cancellationToken)
        {



            var item = await _context.Items.FirstOrDefaultAsync(t => t.Id == command.Id);

            if (item == null)
            {
                throw new NotFoundException($"Catalog item with Id: {command.Id} does not exist in database!");
            }

            var deleteResult = await DeleteItem(item.Id);
            //item.IsDeleted = true;
            //item.DeletedAt = dateNow;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task<bool> DeleteItem(Guid itemId)
        {
            var item = await _context.Items.Include(x => x.Members).FirstOrDefaultAsync(t => t.Id == itemId);
            bool result = false;
            if(item != null)
            {
                if (item.IsFolder)
                {
                    foreach(var i in item.Members)
                    {
                        var res = await DeleteItem(i.Id);
                        if (!res){
                            return res;
                        }
                    }
                }
                _context.Items.Remove(item);
                result = true;
            }
            return result;
        }
    }
}
