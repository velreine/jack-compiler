using System.Linq;
using System.Collections.Generic;
using Jack_Compiler.Tokens;

public class Operator {

  private readonly TokenType _value;
  public static HashSet<TokenType> SupportedOperators = new HashSet<TokenType> {
    TokenType.SYMBOL_PLUS,
    TokenType.SYMBOL_MINUS,
    TokenType.SYMBOL_EQUALITY,
    TokenType.SYMBOL_GREATER_THAN,
    TokenType.SYMBOL_LESS_THAN
  };

  public Operator(TokenType value) {
    this._value = value;

    // Check if it is a valid symbol.
    var isValidSymbol = SupportedOperators.Contains(value);

    if(!isValidSymbol) {
      throw new System.Exception("Unknown operation attemped to be constructed: " + this._value);
    }
  }

    public string ToXML(int indentLevel) {
        string indentComp = new string('\t', indentLevel);
        return indentComp + $"<operator>{Lexer.symbolsToStr[_value]}</operator>";
    }
}