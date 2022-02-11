using System.Xml;
using System.Xml.Linq;
using Jack_Compiler.Common;

namespace Jack_Compiler.Tokens
{
  public class Token : ICanBeExpressedAsXML
  {
    public TokenType Type { get; init; }
    public int LineNumber { get; init; }

    private string _stringValue { get; init; }
    public string StringValue
    {
      get
      {

        if (this.Type != TokenType.STRING_CONSTANT && this.Type != TokenType.IDENTIFIER)
        {
          throw new System.Exception("Tried to access StringValue even though this token is not a string constant and not an identifier.");
        }

        return this._stringValue;
      }
    }

    private int? _integerValue { get; init; }
    public int? IntegerValue
    {
      get
      {

        if (this.Type != TokenType.INTEGER_CONSTANT)
        {
          throw new System.Exception("Tried to access IntegerValue even though this token is not a integer constant.");
        }

        return this._integerValue;
      }
    }

    public Token(TokenType type, int lineNumber, string stringValue = null, int? integerValue = null)
    {
      this.Type = type;
      this.LineNumber = lineNumber;

      if (type != TokenType.IDENTIFIER && type != TokenType.STRING_CONSTANT)
      {
        if (stringValue != null)
        {
          throw new System.Exception("StringValue MUST be null if the TokenType is not IDENTIFIER OR STRING_CONSTANT");
        }
      }

      if (type != TokenType.INTEGER_CONSTANT && integerValue != null)
      {
        throw new System.Exception("IntegerValue MUST be null when the TokenType is not INTEGER_CONSTANT");
      }

      this._stringValue = stringValue;
      this._integerValue = integerValue;
    }

    public string ToXML(int indentLevel)
    {
      string indent = new string('\t', indentLevel);
      if (Type == TokenType.INTEGER_CONSTANT)
      {
        return indent + $"<int-literal>{IntegerValue}</int-literal>";
      }
      if (Type == TokenType.STRING_CONSTANT)
      {
        return indent + $"<string>{StringValue.Replace("\"", "&quot;")}</string>";
      }
      if (Lexer.symbolsToStr.TryGetValue(Type, out char symbol))
      {
        return indent + $"<symbol>{symbol}</symbol>";
      }
      if (Lexer.keywordsToStr.TryGetValue(Type, out string keyword))
      {
        return indent + $"<keyword>{keyword}</keyword>";
      }
      if (Type == TokenType.IDENTIFIER)
      {
        return indent + $"<identifier>{StringValue}</identifier>";
      }

      // we require the tokenizer to output these tokens as &lt;, &gt;, &quot;, and &amp;,
      if (Type == TokenType.SYMBOL_AMPERSAND) {
          return indent + "<symbol>&amp;</symbol>";
      }
      if (Type == TokenType.SYMBOL_LESS_THAN) {
          return indent + "<symbol>&lt;</symbol>";
      }
      if (Type == TokenType.SYMBOL_GREATER_THAN) {
          return indent + "<symbol>&gt;</symbol>";
      }

      throw new System.Exception(/*#ConfusedAF*/"Could not recognise XML gender uwu " + "Could not XML'ize ");
    }
  }
}