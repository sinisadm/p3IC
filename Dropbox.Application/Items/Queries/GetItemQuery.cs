using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dropbox.Application.Common.Interfaces;
using Dropbox.Application.Items.Queries;

namespace Dropbox.Application.Items.Queries
{
    public class GetItemQuery : IRequest<ItemDto>
    {
        public GetItemQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

    }

    public class GetItemQueryHandler : IRequestHandler<GetItemQuery, ItemDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetItemQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ItemDto> Handle(GetItemQuery query, CancellationToken cancellationToken)
        {
            var item = await _context.Items
                .Include(x => x.Members)
                .Include(x => x.ParentItem)
                .Include(x => x.UserDevice)
                    .ThenInclude(x => x.Device)
                .Where(x => x.Id == query.Id)
                .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return item;
        }
    }
}
