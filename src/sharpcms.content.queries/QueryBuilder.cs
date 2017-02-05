using System.Collections.Generic;
using sharpcms.content.model.queries;
using sharpcms.content.model.queries.enums;

namespace sharpcms.content.queries
{
    public class QueryBuilder
    {
        private readonly List<string> _author = new List<string>();

        private readonly List<string> _section = new List<string>();

        private readonly List<string> _target = new List<string>();

        private readonly List<string> _content = new List<string>();

        private readonly List<string> _tags = new List<string>();

        private ComparisonType _queryType = ComparisonType.Contains;

        private LogicalOperatorType _operatorType = LogicalOperatorType.Or;

        private string _orderBy = "ORDER BY [Updated] DESC";

        private int _skip = 0;

        private int _take = 25;

        public static QueryBuilder New()
        {
            return new QueryBuilder();
        }

        public QueryBuilder WithTarget(string target)
        {
            _target.Add(target);
            return this;
        }

        public QueryBuilder WithSection(string section)
        {
            _section.Add(section);
            return this;
        }

        public QueryBuilder WithAuthor(string author)
        {
            _author.Add(author);
            return this;
        }

        public QueryBuilder WithContent(string content)
        {
            _content.Add(content);
            return this;
        }

        public QueryBuilder WithTags(string tags)
        {
            _tags.Add(tags);
            return this;
        }

        public QueryBuilder WithAndOperator()
        {
            _operatorType = LogicalOperatorType.And;
            return this;
        }

        public QueryBuilder WithOrOperator()
        {
            _operatorType = LogicalOperatorType.Or;
            return this;
        }

        public QueryBuilder OrderBy(string orderBy)
        {
            _orderBy = orderBy;
            return this;
        }

        public QueryBuilder Exact()
        {
            _queryType = ComparisonType.Exact;
            return this;
        }

        public QueryBuilder Contains()
        {
            _queryType = ComparisonType.Contains;
            return this;
        }

        public QueryBuilder StartsWith()
        {
            _queryType = ComparisonType.StartsWith;
            return this;
        }

        public QueryBuilder EndsWith()
        {
            _queryType = ComparisonType.EndsWith;
            return this;
        }

        public QueryBuilder Skip(int skip)
        {
            this._skip = skip;
            return this;
        }

        public QueryBuilder Take(int take)
        {
            this._take = take;
            return this;
        }

        public ContentFragmentPagedQueryModel Build()
        {
            return new ContentFragmentPagedQueryModel
            {
                Query = new ContentFragmentQueryModel
                {
                    Target = _target,
                    Section = _section,
                    Author = _author,
                    Content = _content,
                    Tags = _tags,
                    QueryType = _queryType,
                    OrderBy = _orderBy,
                    OperatorType = _operatorType
                },
                Skip = _skip,
                Take = _take
            };
        }
    }
}