using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dropbox.Application.Common.Exceptions;
using Dropbox.Application.Common.Interfaces;

namespace Dropbox.Application.Users.Commands
{
    public class DeleteUserCommand : IRequest
    {
        public DeleteUserCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteUserCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {

            var dateNow = DateTime.Now;

            var user = await _context.Users.FirstOrDefaultAsync(t => t.Id == command.Id);

            if (user == null)
            {
                throw new NotFoundException($"Catalog item with Id: {command.Id} does not exist in database!");
            }

            user.IsDeleted = true;
            user.DeletedAt = dateNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
