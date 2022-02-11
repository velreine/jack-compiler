using Jack_Compiler.Tokens;
using Jack_Compiler.Common;
using System.Text;

public enum StatementType
{
  Let,
  While,
  If,
  ReturnNothing,
  ReturnSomething,
}

// This is based
public class Statement : ICanBeExpressedAsXML
{
  public StatementType Type { get; init; } // let | while | if
  public Expr Expression { get; init; } // if (>>>Conditional<<<)  OR let varName = >>>expression<<<
  public StatementList Statements { get; init; } // while(expression) { >>>...statements<<< }
  public string VarName { get; init; } // let >>>varName<<< = expression

    // Based? Based on what?
    // https://www.youtube.com/watch?v=LrNu-SuFF_o
    private Statement () : base() {
        // Constructor skal være her ellers bliver Camilla sur.
    }

  public static Statement WhileStatement(Expr conditional, StatementList subStatements) {
    return new Statement {
        Type = StatementType.While,
        Expression = conditional,
        Statements = subStatements,
    };
  }

  public static Statement IfStatement(Expr conditional, StatementList subStatements) { 
    return new Statement {
        Type = StatementType.If,
        Expression = conditional,
        Statements = subStatements,
    };
  }

  // This is wrong, but don't mind it
  public static Statement LetStatement(string varName, Expr assignmentExpression) {
    return new Statement {
        Type = StatementType.Let,
        VarName = varName,
        Expression = assignmentExpression,
    };
  }

  public static Statement ReturnStatement() {
    return new Statement {
        Type = StatementType.ReturnNothing,
    };
  }

  public static Statement ReturnStatementExpression(Expr assignmentExpression) {
    return new Statement {
        Type = StatementType.ReturnSomething,
        Expression = assignmentExpression,
    };
  }

  public string ToXML(int indentLevel)
  {
    string indentComp = new string('\t', indentLevel);
    var sb = new StringBuilder();

    switch (this.Type) {
      case StatementType.Let:
        sb.Append(indentComp);
        sb.AppendLine("<let-statement>");

        // let varName = Expression.
        sb.AppendLine(new string('\t', indentLevel + 1) + $"<var-name>{VarName}</var-name>");

        sb.Append(Expression.ToXML(indentLevel + 1));

        sb.Append(indentComp);
        sb.AppendLine("</let-statement>");
        break;
      case StatementType.While:
        sb.Append(indentComp);
        sb.AppendLine("<while-statement>");

        sb.Append(Expression.ToXML(indentLevel + 1));

        sb.Append(Statements.ToXML(indentLevel + 1));

        sb.Append(indentComp);
        sb.AppendLine("</while-statement>");
        break;
      case StatementType.If:
        sb.Append(indentComp);
        sb.AppendLine("<if-statement>");

        sb.Append(Expression.ToXML(indentLevel + 1));

        sb.Append(Statements.ToXML(indentLevel + 1));

        sb.Append(indentComp);
        sb.AppendLine("</if-statement>");
        break;
      case StatementType.ReturnNothing:
        sb.Append(indentComp);
        sb.AppendLine("<return/>");
        break;
      case StatementType.ReturnSomething:
        sb.Append(indentComp);
        sb.AppendLine("<return>");

        sb.Append(Expression.ToXML(indentLevel + 1));

        sb.Append(indentComp);
        sb.AppendLine("</return>");
        break;
    }

    return sb.ToString();
  }
}