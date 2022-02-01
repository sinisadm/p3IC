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

namespace Dropbox.Application.Users.Commands
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string Email { get; set; }
        public uint QuotaLimit { get; set; } = 0;
        public uint QuotaUsed { get; set; } = 0;
    }

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(t => t.Email).NotEmpty();
            //RuleFor(t => t.QuotaLimit).NotEmpty();
        }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateUserCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            try
            {

                var userExists = await _context.Users.Where(t => t.Email == command.Email).FirstOrDefaultAsync();

                if (userExists != null)
                {
                    throw new DuplicateItemException($"User with emai {command.Email} already exists!");
                }

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = command.Email,
                    QuotaLimit = command.QuotaLimit,
                    OuotaUsed = command.QuotaUsed
                };

                _context.Users.Add(user);

                await _context.SaveChangesAsync(cancellationToken);

                return user.Id;

            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
