using System.Collections.Generic;

namespace sharpcms.content.model.queries
{
    public class ContentFragmentPagedQueryResultsModel
    {
        public ContentFragmentPagedQueryModel Query { get; set; }

        public int Total { get; set; }

        public IEnumerable<ContentFragmentModel> Results { get; set; }
    }
}