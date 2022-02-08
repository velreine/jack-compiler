public abstract class AbstractStatement
{
    public abstract string Keyword { get; init; }

    public abstract string ToXML();
}
