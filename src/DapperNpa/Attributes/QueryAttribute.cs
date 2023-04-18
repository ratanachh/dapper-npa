namespace DapperNpa.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class QueryAttribute : Attribute
{
    public string Sql { get; }

    public QueryAttribute(string sql)
    {
        Sql = sql;
    }
}