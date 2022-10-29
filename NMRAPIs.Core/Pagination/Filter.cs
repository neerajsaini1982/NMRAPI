using System.Collections.Generic;

namespace NMRAPIs.Core.Pagination
{
    public enum LogicalOperator
    {
        Or = 1,
        And = 2,
        Not = 3
    }

    public enum RelationalOperator
    {
        Like,
        Equal,
        NotEqual,
        Less,
        LessOrEqual,
        GreaterOrEqual,
        Greater,
        InRange
    }

    public class BaseFilter
    {
        public string Property { get; set; }
        public RelationalOperator RelationalOperator { get; set; }
        public string Value { get; set; }        
    }

    public class Filter : BaseFilter
    {
        public LogicalOperator LogicalOperator { get; set; }
        public int? CustomFieldId { get; set; }
    }

    public class ParametrizedFilter
    {
        public ICollection<Filter> Filters { get; set; }
    }
}
