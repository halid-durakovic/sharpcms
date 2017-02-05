using sharpcms.content.model;

namespace sharpcms.content
{
    public interface IContentFragmentService
    {
        void CreateIfDoesNotExist(string name);

        void DeleteIfDoesExist(string name);

        void Insert(string name, ContentFragmentModel contentFragment);

        void Update(string name, ContentFragmentModel contentFragment);

        void Delete(string name, ContentFragmentModel contentFragment);

        ContentFragmentModel GetById(string name, int id);
    }
}