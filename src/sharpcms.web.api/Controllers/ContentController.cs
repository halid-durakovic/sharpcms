using Microsoft.AspNetCore.Mvc;
using sharpcms.content;
using sharpcms.content.model;
using sharpcms.content.model.queries;
using sharpcms.content.queries;

namespace sharpcms.web.api.Controllers
{
    [Route("api/[controller]")]
    public class ContentController : Controller
    {
        public static string Db = "sharpcms";

        private readonly IContentFragmentService _contentFragments;

        private readonly IContentFragmentQueryService _contentFragmentsQuery;

        public ContentController(
            IContentFragmentService contentFragments, 
            IContentFragmentQueryService contentFragmentsQuery)
        {
            _contentFragments = contentFragments;

            _contentFragmentsQuery = contentFragmentsQuery;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var query = QueryBuilder.New().WithContent(string.Empty).Build();

            var results = _contentFragmentsQuery.Find(Db, query);

            return Ok(results);
        }

        [HttpPost("q")]
        public IActionResult Query([FromBody] ContentFragmentPagedQueryModel query)
        {
            var results = _contentFragmentsQuery.Find(Db, query);

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _contentFragments.GetById(Db, id);

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ContentFragmentModel fragment)
        {
            _contentFragments.Insert(Db, fragment);

            return Ok(fragment);
        }

        [HttpPut]
        public IActionResult Put([FromBody] ContentFragmentModel fragment)
        {
            _contentFragments.Update(Db, fragment);

            return Ok(fragment);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var fragment = _contentFragments.GetById(Db, id);

            _contentFragments.Delete(Db, fragment);

            return Ok(fragment);
        }
    }
}