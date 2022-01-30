using System.Collections.Generic;

namespace Dropbox.Application.Common
{
    public class QueryOptions
    {
        public int Page { get; set; } = 1;
        //public int? Skip { get; set; } = 0;
        public int? Take { get; set; }
        public string Sort { get; set; }
        //public string Group { get; set; }
    }
}
