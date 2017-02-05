using System.Collections.Generic;
using System.Linq;
using Dapper;
using sharpcms.content.model;
using sharpcms.content.model.queries;
using sharpcms.content.model.queries.enums;
using sharpcms.content.queries;
using sharpcms.content.queries.extensions;
using sharpcms.database;

namespace sharpcms.content
{
    public class ContentFragmentQueryService : IContentFragmentQueryService
    {
        private readonly DbConnectionService _db;

        public ContentFragmentQueryService() : this(new DbConnectionService())
        {
        }

        public ContentFragmentQueryService(DbConnectionService db)
        {
            _db = db;
        }

        public ContentFragmentPagedQueryResultsModel Find(string name, ContentFragmentPagedQueryModel query) 
        {
            using (var c = _db.GetConnection(name))
            {
                var fragments = c.Connection.Query<ContentFragmentModel>($"{getContentFragmentQuerySql(query)}");

                var total = c.Connection.Query<int>($"{getContentFragmentCountQuerySql(query)}").First();

                var results = new ContentFragmentPagedQueryResultsModel()
                {
                    Query = query, 
                    Results = fragments,
                    Total = total
                };

                return results;
            }
        }

        private string getContentFragmentQuerySql(ContentFragmentPagedQueryModel query)
        {
            var filter = getContentFragmentQueryFilterSql(query);

            var start = query.Skip + 1;

            var end = query.Take + query.Skip + 1;

            return $@"SELECT  
	                *
                FROM 
                ( 
	                SELECT    
		                ROW_NUMBER() OVER ( {query.Query.OrderBy} ) AS RowNumber, *
	                FROM      
		                [ContentFragment]
	                WHERE     
		                Deleted IS NULL AND ({filter})
                ) AS Result
                WHERE   
	                RowNumber >= {start}
                    AND RowNumber < {end}
                ORDER BY 
	                RowNumber";
        }

        private string getContentFragmentQueryFilterSql(ContentFragmentPagedQueryModel query)
        {
            var filterItems = new List<string>();

            if (query.Query.Target.Count != 0)
                foreach(var target in query.Query.Target)
                    filterItems.Add($"[Target] {query.Query.GetOperatorSql()} {query.Query.GetNormalisedStringSql(target)}");

            if (query.Query.Section.Count != 0)
                foreach (var section in query.Query.Section)
                    filterItems.Add($"[Section] {query.Query.GetOperatorSql()} {query.Query.GetNormalisedStringSql(section)}");

            if (query.Query.Author.Count != 0)
                foreach (var author in query.Query.Author)
                    filterItems.Add($"[Author] {query.Query.GetOperatorSql()} {query.Query.GetNormalisedStringSql(author)}");

            if (query.Query.Content.Count != 0)
                foreach (var content in query.Query.Content)
                    filterItems.Add($"[Content] {query.Query.GetOperatorSql()} {query.Query.GetNormalisedStringSql(content)}");

            if (query.Query.Tags.Count != 0)
                foreach (var tag in query.Query.Tags)
                    filterItems.Add($"[Tags] {query.Query.GetOperatorSql()} {query.Query.GetNormalisedStringSql(tag)}");

            if (query.Query.OperatorType == LogicalOperatorType.And)
                return string.Join(" AND ", filterItems);

            return string.Join(" OR ", filterItems);
        }

        private string getContentFragmentCountQuerySql(ContentFragmentPagedQueryModel query)
        {
            var filter = getContentFragmentQueryFilterSql(query);

            return $@"SELECT    
		            COUNT('N')
	            FROM      
		            [ContentFragment]
	            WHERE     
		            Deleted IS NULL AND ({filter})";
        }
    }
}