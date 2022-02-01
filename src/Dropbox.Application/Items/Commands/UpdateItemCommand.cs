using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Dropbox.Application.Common.Exceptions;
using Dropbox.Application.Common.Interfaces;

namespace Dropbox.Application.Items.Commands
{
    public class UpdateItemCommand : IRequest
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; }
    }

    public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
    {
        public UpdateItemCommandValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
            RuleFor(t => t.ItemName).NotEmpty();
        }
    }

    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateItemCommand command, CancellationToken cancellationToken)
        {
            var item = await _context.Items
                .FirstOrDefaultAsync(t => t.Id == command.Id);

            if (item == null)
            {
                throw new NotFoundException($"Cannot find CatalogItem with Id: {command.Id}");
            }

            item.ItemName = command.ItemName;
            item.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
