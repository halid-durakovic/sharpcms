using sharpcms.content.model.queries;
using sharpcms.content.model.queries.enums;
using sharpcms.content.queries.exceptions;

namespace sharpcms.content.queries.extensions
{
    public static class ContentFragmentQueryExtensions
    {
        public static string GetOperatorSql(this ContentFragmentQueryModel query)
        {
            switch (query.QueryType)
            {
                case ComparisonType.Contains:

                case ComparisonType.StartsWith:

                case ComparisonType.EndsWith:
                    return "LIKE";

                case ComparisonType.Exact:
                    return "=";
            }

            throw new UnknownContentFragmentQueryTypeException();
        }

        public static string GetNormalisedStringSql(this ContentFragmentQueryModel query, string denormalisedValue)
        {
            var normalisedValue = denormalisedValue.Replace("'", "''");

            switch (query.QueryType)
            {
                case ComparisonType.Contains:
                    return $"N'%{normalisedValue}%'";

                case ComparisonType.StartsWith:
                    return $"N'{normalisedValue}%'";

                case ComparisonType.EndsWith:
                    return $"N'%{normalisedValue}'";

                case ComparisonType.Exact:
                    return $"N'{normalisedValue}'";
            }

            throw new UnknownContentFragmentQueryTypeException();
        }
    }
}