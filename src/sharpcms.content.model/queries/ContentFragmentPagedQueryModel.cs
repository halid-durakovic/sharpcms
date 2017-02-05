namespace sharpcms.content.model.queries
{
    public class ContentFragmentPagedQueryModel
    {
        public ContentFragmentQueryModel Query { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

    }
}