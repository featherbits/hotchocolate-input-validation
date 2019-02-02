using HotChocolate;
using HotChocolate.Execution;

namespace HotChocolate
{
    public static class SchemaExtensions
    {
        public static ReadOnlyQueryResult ExecuteQuery(this ISchema schema, string query)
        {
            var queryExecuter = schema.MakeExecutable(c => c.UseDefaultPipeline());
            var result = (ReadOnlyQueryResult)queryExecuter.Execute(query);
            return result;
        }
    }
}