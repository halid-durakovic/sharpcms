using GenFu;
using NUnit.Framework;
using sharpcms.content.model;

namespace sharpcms.content.tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ContentFragmentServiceTests
    {
        private ContentFragmentService _contentFragment;

        private string _name = "fragments";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _contentFragment = new ContentFragmentService();

            _contentFragment.CreateIfDoesNotExist(_name);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _contentFragment.DeleteIfDoesExist(_name);
        }

        [Test]
        public void Should_be_able_to_create_content_fragment()
        {
            var contentFragment = A.New<ContentFragmentModel>();

            _contentFragment.Insert(_name, contentFragment);
        }

        [Test]
        public void Should_be_able_to_update_content_fragment()
        {
            var contentFragment = A.New<ContentFragmentModel>();

            _contentFragment.Insert(_name, contentFragment);

            var updatedContentFragment = A.New<ContentFragmentModel>();

            updatedContentFragment.Id = contentFragment.Id;

            _contentFragment.Update(_name, updatedContentFragment);

            Assert.That(updatedContentFragment.Created, Is.Not.EqualTo(updatedContentFragment.Updated));
        }

        [Test]
        public void Should_be_able_to_delete_content_fragment()
        {
            var contentFragment = A.New<ContentFragmentModel>();

            _contentFragment.Insert(_name, contentFragment);

            _contentFragment.Delete(_name, contentFragment);

            var result = _contentFragment.GetById(_name, contentFragment.Id);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Should_be_able_to_get_content_fragment_by_id()
        {
            var contentFragment = A.New<ContentFragmentModel>();

            _contentFragment.Insert(_name, contentFragment);

            var result = _contentFragment.GetById(_name, contentFragment.Id);

            Assert.That(result, Is.EqualTo(contentFragment));
        }
    }
}
