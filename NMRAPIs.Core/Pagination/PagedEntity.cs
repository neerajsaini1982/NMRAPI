using System.Collections.Generic;

namespace NMRAPIs.Core.Pagination
{
    public class PagedEntity<T>
    {
        public IEnumerable<T> Results { get; set; }

        public int TotalCount { get; set; }
    }
}
