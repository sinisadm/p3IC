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

namespace Dropbox.Application.Devices.Queries
{
    public class GetDevicesQuery : IRequest<List<DeviceDto>>
    {
        public GetDevicesQuery(DeviceSpecification specification, QueryOptions queryOptions)
        {
            Specification = specification;
            QueryOptions = queryOptions;
        }

        public DeviceSpecification Specification { get; set; }
        public QueryOptions QueryOptions { get; set; }
    }

    public class GetDevicesQueryHandler : IRequestHandler<GetDevicesQuery, List<DeviceDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetDevicesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DeviceDto>> Handle(GetDevicesQuery query, CancellationToken cancellationToken)
        {
            var catalogItems = await _context.Devices
                .Where(query.Specification.Predicate)
                .ProjectTo<DeviceDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return catalogItems;
        }
    }
}
