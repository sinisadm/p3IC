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

namespace Dropbox.Application.Items.Queries
{
    public class GetItemsQuery : IRequest<List<ItemDto>>
    {
        public GetItemsQuery(ItemSpecification specification, QueryOptions queryOptions)
        {
            Specification = specification;
            QueryOptions = queryOptions;
        }

        public ItemSpecification Specification { get; set; }
        public QueryOptions QueryOptions { get; set; }
    }

    public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, List<ItemDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetItemsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ItemDto>> Handle(GetItemsQuery query, CancellationToken cancellationToken)
        {
            var items = await _context.Items
                .Include(x => x.ParentItem)
                .Include(x => x.UserDevice)
                    .ThenInclude(x => x.Device)
                .Where(query.Specification.Predicate)
                .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
                .Take(10)
                .ToListAsync(cancellationToken);

            return items;
        }
    }
}
