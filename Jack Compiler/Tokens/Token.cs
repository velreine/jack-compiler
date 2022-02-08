using System.Xml;
using System.Xml.Linq;

namespace Jack_Compiler.Tokens
{
  public class Token : ICanBeExpressedAsXML
  {
    public TokenType Type { get; init; }

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

    public Token(TokenType type, string stringValue = null, int? integerValue = null)
    {
      this.Type = type;

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

    public string ToXML()
    {
      if (Type == TokenType.INTEGER_CONSTANT)
      {
        return "<int_literal>" + IntegerValue + "</int_literal>";
      }
      if (Type == TokenType.STRING_CONSTANT)
      {
        return "<string>" + StringValue + "</string>";
      }
      if (Lexer.symbolsToStr.TryGetValue(Type, out char symbol))
      {
        return "<symbol>" + symbol + "</symbol>";
      }
      if (Lexer.keywordsToStr.TryGetValue(Type, out string keyword))
      {
        return "<keyword>" + keyword + "</keyword>";
      }
      if (Type == TokenType.IDENTIFIER)
      {
        return "<identifier>" + StringValue + "</identifier>";
      }

      throw new System.Exception(/*#ConfusedAF*/"Could not recognise XML gender " + "Could not XML'ize ");
    }
  }
}