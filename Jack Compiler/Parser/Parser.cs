using Jack_Compiler.Tokens;
using System.Linq;

public class Parser
{

  private TokenList _list { get; init; }
  private int _currentToken = 0;
  public Parser(TokenList list)
  {
    this._list = list;
  }

  private Token GetNextToken()
  {
    if (!HasNextToken())
    {
      return null;
    }
    Token current = _list[_currentToken];
    _currentToken++;
    return current;
  }

  private Token PeekToken()
  {
    if (!HasNextToken())
    {
      return null;
    }
    return _list[_currentToken];
  }

  private bool HasNextToken()
  {
    return _currentToken < this._list.Count;
  }

  private Operator ParseOp()
  {
    return new Operator(GetNextToken().Type);
  }

  private Term ParseTerm()
  {
    return new Term(GetNextToken());
  }

  // Expr = term (op term)?
  private Expr ParseExpr()
  {
    Term left = ParseTerm();
    Token next = PeekToken();

    // Check if the next token is an operation (+, -, =, >, <) etc...
    if (next is not null && Operator.SupportedOperators.Contains(next.Type))
    {
      return new Expr(left, ParseOp(), ParseTerm());
    }
    else
    {
      // If it isn't this must be a "simple" expression. Of the type "term".
      return new Expr(left, null, null);
    }
  }

  // let varName = 2;
  // let varName = 2 + foo;
  // let varName = foo;
  private Statement ParseLetStatement()
  {
    Token next = GetNextToken();
    if (next.Type != TokenType.KEYWORD_LET)
    {
      throw new System.Exception("let expected when calling parse let statement");
    }

    next = GetNextToken();
    if (next.Type != TokenType.IDENTIFIER)
    {
      throw new System.Exception("expected var name");
    }
    string varName = next.StringValue;

    next = GetNextToken();
    if (next.Type != TokenType.SYMBOL_EQUALITY)
    {
      throw new System.Exception("expected =");
    }
    Expr expr = ParseExpr();

    next = GetNextToken();
    if (next.Type != TokenType.SYMBOL_SEMI_COLON)
    {
      throw new System.Exception("expected ;");
    }

    return Statement.LetStatement(varName, expr);
  }

 // nicky skrive noget smart her
  private Statement ParseIfStatement()
  {
    Token next = GetNextToken();
    if (next.Type != TokenType.KEYWORD_IF)
    {
      throw new System.Exception("if expected when calling parse if statement");
    }

    next = GetNextToken();
    if (next.Type == TokenType.SYMBOL_LEFT_PARENTHESIS)
    {
      throw new System.Exception("expected (");
    }

    Expr expr = ParseExpr();

    next = GetNextToken();
    if (next.Type == TokenType.SYMBOL_RIGHT_PARENTHESIS)
    {
      throw new System.Exception("expected )");
    }

    next = GetNextToken();
    if (next.Type == TokenType.SYMBOL_LEFT_BRACE)
    {
      throw new System.Exception("expected {");
    }
    StatementList statements = ParseStatementList();

    next = GetNextToken();
    if (next.Type == TokenType.SYMBOL_RIGHT_BRACE)
    {
      throw new System.Exception("expected }");
    }

    return Statement.IfStatement(expr, statements);
  }

    // nicky skrive noget smart her
    private Statement ParseWhileStatment(){
    Token next = GetNextToken();
    if (next.Type != TokenType.KEYWORD_WHILE)
    {
      throw new System.Exception("while expected when calling parse while statement");
    }

    next = GetNextToken();
    if (next.Type == TokenType.SYMBOL_LEFT_PARENTHESIS)
    {
      throw new System.Exception("expected (");
    }

    Expr expr = ParseExpr();

    next = GetNextToken();
    if (next.Type == TokenType.SYMBOL_RIGHT_PARENTHESIS)
    {
      throw new System.Exception("expected )");
    }

    next = GetNextToken();
    if (next.Type == TokenType.SYMBOL_LEFT_BRACE)
    {
      throw new System.Exception("expected {");
    }

    StatementList statements = ParseStatementList();

    next = GetNextToken();
    if (next.Type == TokenType.SYMBOL_RIGHT_BRACE)
    {
      throw new System.Exception("expected }");
    }

    return Statement.WhileStatement(expr, statements);
  }

  private Statement ParseStatement()
  {
    Token t = PeekToken();
    if (t.Type == TokenType.KEYWORD_LET)
    {
        return ParseLetStatement();
    }
    else if (t.Type == TokenType.KEYWORD_IF)
    {
        return ParseIfStatement();
    }
    else if (t.Type == TokenType.KEYWORD_WHILE)
    {
        return ParseWhileStatment();
    }
    return null;
  }

  private StatementList ParseStatementList() {
      return new StatementList();
  }

  public /*JackAST*/Statement ParseTokens()
  {


    // sindsygt nu R vi done.

    return ParseStatement();
  }

}