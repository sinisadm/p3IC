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
    public class GetFilesQuery : IRequest<List<FileDto>>
    {
        public GetFilesQuery(FileSpecification specification, QueryOptions queryOptions)
        {
            Specification = specification;
            QueryOptions = queryOptions;
        }

        public FileSpecification Specification { get; set; }
        public QueryOptions QueryOptions { get; set; }
    }

    public class GetFilesQueryHandler : IRequestHandler<GetFilesQuery, List<FileDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetFilesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<FileDto>> Handle(GetFilesQuery query, CancellationToken cancellationToken)
        {
            var items = await _context.Items
                .Include(x => x.ParentItem)
                .Include(x => x.UserDevice)
                    .ThenInclude(x => x.Device)
                .Where(query.Specification.Predicate)
                .ProjectTo<FileDto>(_mapper.ConfigurationProvider)
                .Take(10)
                .ToListAsync(cancellationToken);

            return items;
        }
    }
}
