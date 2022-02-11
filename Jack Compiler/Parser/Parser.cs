using Jack_Compiler.Tokens;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

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

  private Token ExpectToken(TokenType type)
  {
    if (!HasNextToken())
    {
      throw new Exception($"expected token of type: {type.ToString()}, got nothing.");
    }

    var next = GetNextToken();
    if (next.Type == type)
    {
      return next;
    }
    throw new Exception($"(line {next.LineNumber}) - expected token of type: {type.ToString()} got: {next.Type.ToString()}");
  }

  // chack if token is type
  private bool CheckToken(TokenType type)
  {
    return HasNextToken() && PeekToken().Type == type;
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
      return new Expr(left);
    }
  }

  // let varName = 2;
  // let varName = 2 + foo;
  // let varName = foo;
  private Statement ParseLetStatement()
  {
    ExpectToken(TokenType.KEYWORD_LET);

    Token next = ExpectToken(TokenType.IDENTIFIER);
    string varName = next.StringValue;

    ExpectToken(TokenType.SYMBOL_EQUALITY);

    Expr expr = ParseExpr();

    ExpectToken(TokenType.SYMBOL_SEMI_COLON);

    return Statement.LetStatement(varName, expr);
  }

  // if(blah) { ...do Something }
  // if(1 + 2 = 3) { ...do Something }
  private Statement ParseIfStatement()
  {
    ExpectToken(TokenType.KEYWORD_IF);

    ExpectToken(TokenType.SYMBOL_LEFT_PARENTHESIS);

    Expr expr = ParseExpr();

    ExpectToken(TokenType.SYMBOL_RIGHT_PARENTHESIS);

    var block = ParseStatementBlock();

    return Statement.IfStatement(expr, block);
  }

  // nicky skrive noget smart her
  private Statement ParseWhileStatement()
  {
    ExpectToken(TokenType.KEYWORD_WHILE);

    ExpectToken(TokenType.SYMBOL_LEFT_PARENTHESIS);

    Expr expr = ParseExpr();

    ExpectToken(TokenType.SYMBOL_RIGHT_PARENTHESIS);

    var block = ParseStatementBlock();

    return Statement.WhileStatement(expr, block);
  }

  private Statement ParseStatement()
  {
    Token t = PeekToken();

    switch (t.Type)
    {
      case TokenType.KEYWORD_LET:
        return ParseLetStatement();
      case TokenType.KEYWORD_IF:
        return ParseIfStatement();
      case TokenType.KEYWORD_WHILE:
        return ParseWhileStatement();
      case TokenType.KEYWORD_RETURN:
        return ParseReturnStatement();
      default:
        throw new System.Exception("Could not parse statement, expected: let, if or while. Got: " + t.Type.ToString());
    }
  }

  // var's should only be found in methods.
  // var int foobar;
  private VariableDeclaration ParseVarDeclaration()
  {
    ExpectToken(TokenType.KEYWORD_VAR);
    var argType = GetNextToken();
    var argName = ExpectToken(TokenType.IDENTIFIER);
    ExpectToken(TokenType.SYMBOL_SEMI_COLON);
    if (argType.Type == TokenType.IDENTIFIER)
    {
      return VariableDeclaration.ClassVar(argName.StringValue, argType.StringValue, VariableType.Var);
    }
    else
    {
      return VariableDeclaration.Primitive(argName.StringValue, typeMap[argType.Type], VariableType.Var);
    }
  }

  // class Program { field int foobar; static int Barbuzz;  }
  private VariableDeclaration ParseFieldDeclaration()
  {
    ExpectToken(TokenType.KEYWORD_FIELD);
    var argType = GetNextToken();
    var argName = ExpectToken(TokenType.IDENTIFIER);
    if (argType.Type == TokenType.IDENTIFIER)
    {
      return VariableDeclaration.ClassVar(argName.StringValue, argType.StringValue, VariableType.Field);
    }
    else
    {
      return VariableDeclaration.Primitive(argName.StringValue, typeMap[argType.Type], VariableType.Field);
    }
  }

  private VariableDeclaration ParseStaticDeclaration()
  {
    ExpectToken(TokenType.KEYWORD_STATIC);
    var argType = GetNextToken();
    var argName = ExpectToken(TokenType.IDENTIFIER);
    if (argType.Type == TokenType.IDENTIFIER)
    {
      return VariableDeclaration.ClassVar(argName.StringValue, argType.StringValue, VariableType.Static);
    }
    else
    {
      return VariableDeclaration.Primitive(argName.StringValue, typeMap[argType.Type], VariableType.Static);
    }
  }

  // it knows how to parse return stuff.
  // return; <<< void
  // return foobar; <<<                   Expr(left = foobar);
  // return 2 + 3; <<<                    Expr(left = 2, right = 3);
  // return 3 + foo; <<<                  Expr(left = 3, right = foo);
  // return DoIt(foobarish); <<           Expr(func-call, func-name, arg-list)
  private Statement ParseReturnStatement()
  {
    // A return statement should always start with 'return'.
    ExpectToken(TokenType.KEYWORD_RETURN);

    // If the next token is not a semi-colon then an expression should come now.
    if (!CheckToken(TokenType.SYMBOL_SEMI_COLON))
    {
      // if not ';' we expect an expression
      Expr expr = ParseExpr();
      ExpectToken(TokenType.SYMBOL_SEMI_COLON);
      return Statement.ReturnStatementExpression(expr);
    }

    // A return statement should always end with a ';' semi-colon.
    ExpectToken(TokenType.SYMBOL_SEMI_COLON);

    return Statement.ReturnStatement();
  }

  private (StatementList statements, VariableDeclarationList variableDeclarations) ParseClassBlock()
  {
    ExpectToken(TokenType.SYMBOL_LEFT_BRACE);

    var stmts = new StatementList();
    var variableDeclarations = new VariableDeclarationList();

    while (CheckToken(TokenType.KEYWORD_VAR))
    {
      variableDeclarations.Add(ParseVarDeclaration());
    }
    while (HasNextToken() && !CheckToken(TokenType.SYMBOL_RIGHT_BRACE))
    {
      stmts.Add(ParseStatement());
    }

    ExpectToken(TokenType.SYMBOL_RIGHT_BRACE);
    return (stmts, variableDeclarations);
  }

  private (StatementList statements, VariableDeclarationList variableDeclarations) ParseFunctionBlock()
  {
    ExpectToken(TokenType.SYMBOL_LEFT_BRACE);

    var stmts = new StatementList();
    var variableDeclarations = new VariableDeclarationList();

    while (CheckToken(TokenType.KEYWORD_VAR))
    {
      variableDeclarations.Add(ParseVarDeclaration());
    }
    while (HasNextToken() && !CheckToken(TokenType.SYMBOL_RIGHT_BRACE))
    {
      stmts.Add(ParseStatement());
    }

    ExpectToken(TokenType.SYMBOL_RIGHT_BRACE);
    return (stmts, variableDeclarations);
  }

  private StatementList ParseStatementBlock()
  {
    ExpectToken(TokenType.SYMBOL_LEFT_BRACE);

    var stmts = new StatementList();
    var variableDeclarations = new VariableDeclarationList();

    while (CheckToken(TokenType.KEYWORD_VAR))
    {
      variableDeclarations.Add(ParseVarDeclaration());
    }
    while (HasNextToken() && !CheckToken(TokenType.SYMBOL_RIGHT_BRACE))
    {
      stmts.Add(ParseStatement());
    }

    ExpectToken(TokenType.SYMBOL_RIGHT_BRACE);
    return stmts;
  }
  

  private StatementList ParseStatementList()
  {
    var stmts = new StatementList();
    while (HasNextToken())
    {
      stmts.Add(ParseStatement());
    }

    return stmts;
  }

  private readonly static ImmutableDictionary<TokenType, DataType> typeMap = (new Dictionary<TokenType, DataType>() {
    {TokenType.KEYWORD_BOOLEAN, DataType.BOOLEAN},
    {TokenType.KEYWORD_CHAR, DataType.CHAR},
    {TokenType.KEYWORD_INT, DataType.INTEGER},
    {TokenType.KEYWORD_VOID, DataType.VOID},
    {TokenType.IDENTIFIER, DataType.CLASS_REF}
  }).ToImmutableDictionary();

  private FunctionArgument ParseFunctionArgument()
  {
    var argType = GetNextToken();
    var argName = ExpectToken(TokenType.IDENTIFIER);
    if (argType.Type == TokenType.IDENTIFIER)
    {
      return FunctionArgument.ClassArg(argName.StringValue, argType.StringValue);
    }
    else
    {
      return FunctionArgument.Primitive(argName.StringValue, typeMap[argType.Type]);
    }
  }

  private FunctionDeclaration ParseFunction()
  {
    ExpectToken(TokenType.KEYWORD_FUNCTION);

    var returnTypeToken = GetNextToken();
    if (!typeMap.ContainsKey(returnTypeToken.Type))
    {
      throw new System.Exception("Unsupported return type for function: " + returnTypeToken.Type.ToString());
    }

    // Identifier is the name of the function.
    var identifier = ExpectToken(TokenType.IDENTIFIER);

    // function int foo() { ... }
    ExpectToken(TokenType.SYMBOL_LEFT_PARENTHESIS);

    List<FunctionArgument> args = new();
    if (!CheckToken(TokenType.SYMBOL_RIGHT_PARENTHESIS))
    {
      args.Add(ParseFunctionArgument());
      while (CheckToken(TokenType.SYMBOL_COMMA))
      {
        ExpectToken(TokenType.SYMBOL_COMMA);
        args.Add(ParseFunctionArgument());
      }
    }

    ExpectToken(TokenType.SYMBOL_RIGHT_PARENTHESIS);

    var block = ParseFunctionBlock();

    if (returnTypeToken.Type == TokenType.IDENTIFIER)
    {
      return FunctionDeclaration.ClassReturner(identifier.StringValue, returnTypeToken.StringValue, block.statements, block.variableDeclarations, args.ToArray());
    }
    else
    {
      typeMap.TryGetValue(returnTypeToken.Type, out var returnType);
      return FunctionDeclaration.PrimitiveReturner(identifier.StringValue, returnType, block.statements, block.variableDeclarations, args.ToArray());
    }
  }

  private ClassDeclaration ParseClass(){
    ExpectToken(TokenType.KEYWORD_CLASS);
    var className = ExpectToken(TokenType.IDENTIFIER);
    var classblock = ParseClassBlock();
    return null;
  }

  public /*JackAST*/FunctionDeclaration ParseTokens()
  {

    // sindsygt nu R vi done.4
    return ParseFunction();
  }
}