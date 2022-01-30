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

namespace Dropbox.Application.UsersDevices.Queries
{
    public class GetUserDevicesQuery : IRequest<List<UserDeviceDto>>
    {
        public GetUserDevicesQuery(UserDeviceSpecification specification, QueryOptions queryOptions)
        {
            Specification = specification;
            QueryOptions = queryOptions;
        }

        public UserDeviceSpecification Specification { get; set; }
        public QueryOptions QueryOptions { get; set; }
    }

    public class GetUserDevicesQueryHandler : IRequestHandler<GetUserDevicesQuery, List<UserDeviceDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetUserDevicesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserDeviceDto>> Handle(GetUserDevicesQuery query, CancellationToken cancellationToken)
        {
            var catalogItems = await _context.UsersDevices
                .Where(query.Specification.Predicate)
                .ProjectTo<UserDeviceDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return catalogItems;
        }
    }
}
