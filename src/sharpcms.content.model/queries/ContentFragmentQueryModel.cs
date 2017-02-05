using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using sharpcms.content.model.queries.enums;

namespace sharpcms.content.model.queries
{
    public class ContentFragmentQueryModel
    {
        public List<string> Target { get; set; } = new List<string>();

        public List<string> Section { get; set; } = new List<string>();

        public List<string> Author { get; set; } = new List<string>();

        public List<string> Tags { get; set; } = new List<string>();

        public List<string> Content { get; set; } = new List<string>();

        public string OrderBy { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ComparisonType QueryType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public LogicalOperatorType OperatorType { get; set; }
    }
}