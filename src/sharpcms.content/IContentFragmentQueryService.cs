using sharpcms.content.model.queries;
using sharpcms.content.queries;

namespace sharpcms.content
{
    public interface IContentFragmentQueryService
    {
        ContentFragmentPagedQueryResultsModel Find(string name, ContentFragmentPagedQueryModel query);
    }
}