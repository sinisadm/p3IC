using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dropbox.Application.Common;
using Dropbox.Application.Common.Interfaces;
using Dropbox.Application.Common.Specifications;

namespace Dropbox.Application.Users.Queries
{
    public class GetUsersQuery : IRequest<List<UserDto>>
    {
        public GetUsersQuery(UserSpecification specification, QueryOptions queryOptions)
        {
            Specification = specification;
            QueryOptions = queryOptions;
        }

        public UserSpecification Specification { get; set; }
        public QueryOptions QueryOptions { get; set; }
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
        {
            var catalogItems = await _context.Users
                .Where(query.Specification.Predicate)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return catalogItems;
        }
    }
}
