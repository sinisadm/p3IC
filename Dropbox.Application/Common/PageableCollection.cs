using System.Collections.Generic;

namespace Dropbox.Application.Common
{
    public class PageableCollection<TResult>
    {
        public PageableCollection(IEnumerable<TResult> results, long total)
        {
            Results = results;
            Total = total;
        }

        public IEnumerable<TResult> Results { get; }
        public long Total { get; }

    }
}
