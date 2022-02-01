using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dropbox.Application.Common.Interfaces;

namespace Dropbox.Application.UsersDevices.Queries
{
    public class GetUserDeviceQuery : IRequest<UserDeviceDto>
    {
        public GetUserDeviceQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

    }

    public class GetUserQueryHandler : IRequestHandler<GetUserDeviceQuery, UserDeviceDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDeviceDto> Handle(GetUserDeviceQuery query, CancellationToken cancellationToken)
        {
            var catalogItems = await _context.UsersDevices
                .Where(x => x.Id == query.Id)
                .ProjectTo<UserDeviceDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return catalogItems;
        }
    }
}
