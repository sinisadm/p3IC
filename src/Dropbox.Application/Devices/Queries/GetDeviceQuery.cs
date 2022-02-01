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
using System;

namespace Dropbox.Application.Devices.Queries
{
    public class GetDeviceQuery : IRequest<DeviceDto>
    {
        public GetDeviceQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

    public class GetDeviceQueryHandler : IRequestHandler<GetDeviceQuery, DeviceDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetDeviceQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DeviceDto> Handle(GetDeviceQuery query, CancellationToken cancellationToken)
        {
            var catalogItems = await _context.Devices
                .Where(x => x.Id == query.Id)
                .ProjectTo<DeviceDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return catalogItems;
        }
    }
}
