using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dropbox.Application.Common.Interfaces;

namespace Dropbox.Application.Users.Queries
{
    public class GetUserQuery : IRequest<UserDto>
    {
        public GetUserQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            var catalogItems = await _context.Users
                .Where(x => x.Id == query.Id)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return catalogItems;
        }
    }
}
