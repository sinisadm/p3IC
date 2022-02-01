using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Dropbox.Application.Common.Exceptions;
using Dropbox.Application.Common.Interfaces;

namespace Dropbox.Application.Users.Commands
{
    public class UpdateUserCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public uint? QuotaLimit { get; set; }
        public uint? QuotaUsed { get; set; }
    }

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
        }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateUserCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(t => t.Id == command.Id);

            if (user == null)
            {
                throw new NotFoundException($"Cannot find CatalogItem with Id: {command.Id}");
            }

            if(string.IsNullOrEmpty(command.Email.Trim()))
                user.Email = command.Email;
            if (command.QuotaLimit.HasValue)
                user.QuotaLimit = command.QuotaLimit.Value;
            if (command.QuotaUsed.HasValue)
                user.OuotaUsed = command.QuotaUsed.Value;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
