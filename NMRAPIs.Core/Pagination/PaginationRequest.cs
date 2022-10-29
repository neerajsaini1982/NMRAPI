using System.Collections.Generic;

namespace NMRAPIs.Core.Pagination
{
    public class PaginationRequest
    {
        public string OrderBy { get; set; } = null;

        public bool OrderByAscending { get; set; } = true;

        public int? PageIndex { get; set; } = 1;

        public int? PageSize { get; set; } = 10;

        public ICollection<ParametrizedFilter> Filters { get; set; } = null;
    }
}
