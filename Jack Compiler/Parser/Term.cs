using Jack_Compiler.Common;
using Jack_Compiler.Tokens;

public enum TermType
{
  VAR,
  NUM
}

public class Term : ICanBeExpressedAsXML
{
  public TermType Type { get; init; }
  public int? IntValue { get; init; }
  public string StringValue { get; init; }

  public Term(Token t)
  {
    if (t.Type == TokenType.INTEGER_CONSTANT)
    {
      Type = TermType.NUM;
      IntValue = t.IntegerValue;
    }
    else if (t.Type == TokenType.IDENTIFIER)
    {
      Type = TermType.VAR;
      StringValue = t.StringValue;
    }
    else
    {
      throw new System.Exception("Instantiated a term that is not a variable or a number");
    }
  }

  public string ToXML(int indentLevel)
  {
      string indentComp = new string('\t', indentLevel);
    if (Type == TermType.VAR)
    {
        return indentComp + $"<term-var>{StringValue}</term-var>";
    }
    else
    {
        return indentComp + $"<term-num>{IntValue}</term-num>";
    }
  }
}