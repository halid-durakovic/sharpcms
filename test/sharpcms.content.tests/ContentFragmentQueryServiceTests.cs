using System.Linq;
using GenFu;
using NUnit.Framework;
using sharpcms.content.model;
using sharpcms.content.queries;

namespace sharpcms.content.tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ContentFragmentQueryServiceTests
    {
        private ContentFragmentService _contentFragment;

        private ContentFragmentQueryService _contentFragmentQuery;

        private ContentFragmentModel _contentFragment1;

        private ContentFragmentModel _contentFragment2;

        private ContentFragmentModel _contentFragment3;

        private string _name = "fragmentQueries";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _contentFragment = new ContentFragmentService();

            _contentFragmentQuery = new ContentFragmentQueryService();

            _contentFragment.CreateIfDoesNotExist(_name);

            _contentFragment1 = A.New<ContentFragmentModel>();

            _contentFragment.Insert(_name, _contentFragment1);

            _contentFragment2 = A.New<ContentFragmentModel>();

            _contentFragment.Insert(_name, _contentFragment2);

            _contentFragment3 = A.New<ContentFragmentModel>();

            _contentFragment.Insert(_name, _contentFragment3);

        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _contentFragment.DeleteIfDoesExist(_name);
        }

        [Test]
        public void Should_be_able_to_find_exact_by_target()
        {
            var query = QueryBuilder
                .New()
                .WithTarget(_contentFragment1.Target)
                .Exact()
                .Build();

            var queryResults = _contentFragmentQuery.Find(_name, query);

            Assert.That(queryResults.Results.Count(), Is.GreaterThanOrEqualTo(1));

            var result = queryResults.Results.FirstOrDefault(x => x.Equals(_contentFragment1));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Should_be_able_to_find_startswith_by_target()
        {
            var query = QueryBuilder
                .New()
                .WithTarget(_contentFragment1.Target)
                .StartsWith()
                .Build();

            var queryResults = _contentFragmentQuery.Find(_name, query);

            Assert.That(queryResults.Results.Count(), Is.GreaterThanOrEqualTo(1));

            var result = queryResults.Results.FirstOrDefault(x => x.Equals(_contentFragment1));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Should_be_able_to_find_endswith_by_target()
        {
            var query = QueryBuilder
                .New()
                .WithTarget(_contentFragment1.Target)
                .EndsWith()
                .Build();

            var queryResults = _contentFragmentQuery.Find(_name, query);

            Assert.That(queryResults.Results.Count(), Is.EqualTo(1));

            Assert.That(queryResults.Results.First(), Is.EqualTo(_contentFragment1));
        }

        [Test]
        public void Should_be_able_to_find_contains_by_target()
        {
            var query = QueryBuilder
                .New()
                .WithTarget(_contentFragment1.Target)
                .Contains()
                .Build();

            var queryResults = _contentFragmentQuery.Find(_name, query);

            Assert.That(queryResults.Results.Count(), Is.GreaterThanOrEqualTo(1));

            var result = queryResults.Results.FirstOrDefault(x => x.Equals(_contentFragment1));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Should_be_able_to_find_exact_by_author()
        {
            var query = QueryBuilder
                .New()
                .WithAuthor(_contentFragment2.Author)
                .Exact()
                .Build();

            var queryResults = _contentFragmentQuery.Find(_name, query);

            Assert.That(queryResults.Results.Count(), Is.GreaterThanOrEqualTo(1));

            var result = queryResults.Results.FirstOrDefault(x => x.Equals(_contentFragment2));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Should_be_able_to_find_exact_by_section()
        {
            var query = QueryBuilder
                .New()
                .WithSection(_contentFragment3.Section)
                .Exact()
                .Build();

            var queryResults = _contentFragmentQuery.Find(_name, query);

            Assert.That(queryResults.Results.Count(), Is.GreaterThanOrEqualTo(1));

            var result = queryResults.Results.FirstOrDefault(x => x.Equals(_contentFragment3));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Should_be_able_to_find_exact_by_content()
        {
            var query = QueryBuilder
                .New()
                .WithContent(_contentFragment3.Content)
                .Exact()
                .Build();

            var queryResults = _contentFragmentQuery.Find(_name, query);

            Assert.That(queryResults.Results.Count(), Is.GreaterThanOrEqualTo(1));

            var result = queryResults.Results.FirstOrDefault(x => x.Equals(_contentFragment3));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Should_be_able_to_find_exact_by_tags()
        {
            var query = QueryBuilder
                .New()
                .WithTags(_contentFragment3.Tags)
                .Exact()
                .Build();

            var queryResults = _contentFragmentQuery.Find(_name, query);

            Assert.That(queryResults.Results.Count(), Is.GreaterThanOrEqualTo(1));

            var result = queryResults.Results.FirstOrDefault(x => x.Equals(_contentFragment3));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Should_be_able_to_get_multiple_results()
        {
            var query = QueryBuilder
                .New()
                .WithTarget(string.Empty)
                .Contains()
                .Build();

            var queryResults = _contentFragmentQuery.Find(_name, query);

            Assert.That(queryResults.Results.Count(), Is.EqualTo(3));

            Assert.That(queryResults.Results.ElementAt(0), Is.EqualTo(_contentFragment3));

            Assert.That(queryResults.Results.ElementAt(1), Is.EqualTo(_contentFragment2));

            Assert.That(queryResults.Results.ElementAt(2), Is.EqualTo(_contentFragment1));
        }

        [Test]
        public void Should_be_able_to_order_by_id_asc()
        {
            var query = QueryBuilder
                .New()
                .WithTarget(string.Empty)
                .Contains()
                .OrderBy("ORDER BY [Id] ASC")
                .Build();

            var queryResults = _contentFragmentQuery.Find(_name, query);

            Assert.That(queryResults.Results.Count(), Is.EqualTo(3));

            Assert.That(queryResults.Results.ElementAt(0), Is.EqualTo(_contentFragment1));

            Assert.That(queryResults.Results.ElementAt(1), Is.EqualTo(_contentFragment2));

            Assert.That(queryResults.Results.ElementAt(2), Is.EqualTo(_contentFragment3));
        }

        [Test]
        public void Should_be_able_to_page_for_first_result()
        {
            var query = QueryBuilder
                .New()
                .WithTarget(string.Empty)
                .Skip(2)
                .Take(1)
                .Contains()
                .Build();

            var queryResults = _contentFragmentQuery.Find(_name, query);

            Assert.That(queryResults.Results.Count(), Is.GreaterThanOrEqualTo(1));

            Assert.That(queryResults.Results.ElementAt(0), Is.EqualTo(_contentFragment1));
        }

        [Test]
        public void Should_be_able_to_page_for_second_result()
        {
            var query = QueryBuilder
                .New()
                .WithTarget(string.Empty)
                .Skip(1)
                .Take(1)
                .Contains()
                .Build();

            var queryResults = _contentFragmentQuery.Find(_name, query);

            Assert.That(queryResults.Results.Count(), Is.GreaterThanOrEqualTo(1));

            Assert.That(queryResults.Results.ElementAt(0), Is.EqualTo(_contentFragment2));
        }

        [Test]
        public void Should_be_able_to_page_for_third_result()
        {
            var query = QueryBuilder
                .New()
                .WithTarget(string.Empty)
                .Skip(0)
                .Take(1)
                .Contains()
                .Build();

            var queryResults = _contentFragmentQuery.Find(_name, query);

            Assert.That(queryResults.Results.Count(), Is.GreaterThanOrEqualTo(1));

            Assert.That(queryResults.Results.ElementAt(0), Is.EqualTo(_contentFragment3));
        }

        [Test]
        public void Should_match_multiple_items_across_different_fields_using_or_query()
        {
            var query = QueryBuilder
                .New()
                .WithTarget(_contentFragment1.Target)
                .WithAuthor(_contentFragment2.Author)
                .WithSection(_contentFragment3.Section)
                .WithOrOperator()
                .Build();

            var queryResults = _contentFragmentQuery.Find(_name, query);

            Assert.That(queryResults.Total, Is.EqualTo(3));

            Assert.That(queryResults.Results.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Should_match_multiple_items_across_same_fields_using_or_query()
        {
            var query = QueryBuilder
                .New()
                .WithTarget(_contentFragment1.Target)
                .WithTarget(_contentFragment2.Target)
                .WithTarget(_contentFragment3.Target)
                .WithOrOperator()
                .Build();

            var queryResults = _contentFragmentQuery.Find(_name, query);

            Assert.That(queryResults.Total, Is.EqualTo(3));

            Assert.That(queryResults.Results.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Should_return_empy_results_for_multi_fields_using_and()
        {
            var query = QueryBuilder
                .New()
                .WithTarget(_contentFragment1.Target)
                .WithTarget(_contentFragment2.Target)
                .WithTarget(_contentFragment3.Target)
                .WithAndOperator()
                .Build();

            var queryResults = _contentFragmentQuery.Find(_name, query);

            Assert.That(queryResults.Total, Is.EqualTo(0));

            Assert.That(queryResults.Results.Count(), Is.EqualTo(0));
        }
    }
}