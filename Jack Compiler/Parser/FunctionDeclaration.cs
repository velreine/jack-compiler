using System;
using Jack_Compiler.Common;
using System.Text;

public enum FunctionType {
  Method,
  Function,
  Constructor
}

public class FunctionDeclaration : ICanBeExpressedAsXML
{
  public string Name { get; init; } // function void >>>name<<< (int arg1, int arg2)
  public FunctionArgument[] Arguments { get; init; } // function void name >>>(int arg1, int arg2)<<<
  public DataType ReturnType { get; init; } // function >>>void<<< name (int arg1, int arg2)

  public FunctionType Type { get; init; } // >>>function|method|constructor<<< void name (int arg1, int arg2)

  public VariableDeclarationList Variables {get; init; } // function void name (int arg1, int arg2) { >>>...varDeclarations<<< ...Statements}
  public StatementList Statements { get; init; } // function void name (int arg1, int arg2) { >>>...Statements<<<a }

  private string _returnTypeClassName { get; init; }
  public string ReturnTypeClassName
  {
    get
    {
      if (this.ReturnType != DataType.CLASS_REF)
      {
        throw new System.Exception("Cannot access ReturnTypeClassName when ReturnType is not CLASS_REF.");
      }

      return _returnTypeClassName;

    }
    init { _returnTypeClassName = value; }
  } // only when ReturnType == CLASS_REF.

  // Based? Based on what?
  // https://www.youtube.com/watch?v=LrNu-SuFF_o
  private FunctionDeclaration() : base()
  {
    // Constructor skal være her ellers bliver Camilla sur.
    // Camilla when no constructor:
    // https://www.google.com/search?q=angry+spongebob+ip+address&tbm=isch&ved=2ahUKEwj4z42t6fT1AhVCXcAKHS1VB9wQ2-cCegQIABAA&oq=angry+spongebob+ip+address&gs_lcp=CgNpbWcQAzoHCCMQ7wMQJzoFCAAQgAQ6BAgAEENQtgJY7BVg1RZoAHAAeACAAUqIAeIFkgECMTKYAQCgAQGqAQtnd3Mtd2l6LWltZ8ABAQ&sclient=img&ei=7toEYvigJcK6gQatqp3gDQ&bih=969&biw=1920&rlz=1C1GCEA_enDK906DK906#imgrc=epHLPTLgOCa6aM
  }

  // returning function without parameters
  public static FunctionDeclaration PrimitiveReturner(string name, DataType returns, StatementList statements, VariableDeclarationList variables, FunctionArgument[] arguments, FunctionType type)
  {
    return new FunctionDeclaration()
    {
      Type = type,
      Name = name,
      ReturnType = returns,
      Statements = statements,
      Variables = variables,
      Arguments = arguments
    };
  }

  // if returning pointer type/object type without parameters
  public static FunctionDeclaration ClassReturner(string name, string returnTypeClassName, StatementList statements, VariableDeclarationList variables, FunctionArgument[] arguments, FunctionType type)
  {
    return new FunctionDeclaration()
    {
      Name = name,
      ReturnType = DataType.CLASS_REF,
      ReturnTypeClassName = returnTypeClassName,
      Statements = statements,
      Variables = variables,
      Arguments = arguments
    };
  }

  public string ToXML(int indentLevel)
  {
    string indent = new string('\t', indentLevel);
    string indent2 = new string('\t', indentLevel + 1);
    StringBuilder sb = new StringBuilder();

    switch (Type)
    {
      case FunctionType.Method:
        sb.Append(indent);
        sb.AppendLine("<method>");
        break;
      case FunctionType.Constructor:
        sb.Append(indent);
        sb.AppendLine("<constructor>");
        break;
      case FunctionType.Function:
        sb.Append(indent);
        sb.AppendLine("<function>");
        break;
      default:
        throw new Exception("fuck you");
    }

    sb.Append(indent2);
    if (ReturnType == DataType.CLASS_REF)
    {
      sb.AppendLine($"<return-type>{ReturnTypeClassName}</return-type>");
    }
    else
    {
      sb.AppendLine($"<return-type>{ReturnType.ToString()}</return-type>");
    }

    sb.Append(indent2);
    sb.AppendLine($"<name>{Name}</name>");

    if (Arguments.Length > 0)
    {
      sb.Append(indent2);
      sb.AppendLine($"<argument-list>");

      foreach (var arg in Arguments)
      {
        sb.Append(arg.ToXML(indentLevel + 2));
      }

      sb.Append(indent2);
      sb.AppendLine($"</argument-list>");
    }

    sb.Append(Variables.ToXML(indentLevel + 1));

    sb.Append(Statements.ToXML(indentLevel + 1));

    switch (Type)
    {
      case FunctionType.Method:
        sb.Append(indent);
        sb.AppendLine("</method>");
        break;
      case FunctionType.Constructor:
        sb.Append(indent);
        sb.AppendLine("</constructor>");
        break;
      case FunctionType.Function:
        sb.Append(indent);
        sb.AppendLine("</function>");
        break;
      default:
        throw new Exception("fuck you");
    }

    return sb.ToString();
  }
}