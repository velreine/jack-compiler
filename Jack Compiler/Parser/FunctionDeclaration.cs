using System;
using Jack_Compiler.Common;
using System.Text;

public class FunctionArgument : ICanBeExpressedAsXML
{

  public string Name { get; init; }
  private string _typeClassName { get; init; }

  public string TypeClassName
  {
    get
    {
      if (this.DataType != DataType.CLASS_REF)
      {
        throw new System.Exception("Cannot access ReturnTypeClassName when ReturnType is not CLASS_REF.");
      }

      return _typeClassName;

    }
    init { _typeClassName = value; }
  } // only when DataType == CLASS_REF.
  public DataType DataType { get; init; }

  public static FunctionArgument Primitive(string name, DataType dataType)
  {

    // function void doSomething(int a, void b)  <<<< here void does not make sense.
    if (dataType == DataType.VOID)
    {
      throw new System.Exception("VOID is not a valid type for a function argument.");
    }

    // For arguments that are of primitive data types.
    return new FunctionArgument()
    {
      Name = name,
      DataType = dataType
    };
  }

  public static FunctionArgument ClassArg(string name, string typeClassName)
  {
    // For arguments that are of custom/objects data types.
    return new FunctionArgument()
    {
      Name = name,
      TypeClassName = typeClassName,
      DataType = DataType.CLASS_REF
    };
  }

  public string ToXML(int indentLevel)
  {
    string indent = new string('\t', indentLevel);
    string indentMore = new string('\t', indentLevel + 1);
    StringBuilder sb = new StringBuilder();
    sb.Append(indent);
    sb.AppendLine("<argument>");

    if (DataType == DataType.CLASS_REF)
    {
      sb.AppendLine(indentMore + $"<type>{TypeClassName}</type>");
    }
    else
    {
      sb.AppendLine(indentMore + $"<type>{DataType.ToString()}</type>");
    }

    sb.AppendLine(indentMore + $"<var-name>{Name}</var-name>");

    sb.Append(indent);
    sb.AppendLine("</argument>");

    return sb.ToString();
  }
}

public enum DataType
{
  // Primitive return types.
  INTEGER,
  BOOLEAN,
  STRING,
  CHAR,

  // Special.
  VOID, // when the function does not return anything, not valid as parameter
  CLASS_REF, // When the returned type is an object of another class.
}

public class FunctionDeclaration : ICanBeExpressedAsXML
{
  public string Name { get; init; } // function void >>>name<<< (int arg1, int arg2)
  public FunctionArgument[] Arguments { get; init; } // function void name >>>(int arg1, int arg2)<<<
  public DataType ReturnType { get; init; } // function >>>void<<< name (int arg1, int arg2)
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
  public static FunctionDeclaration PrimitiveReturner(string name, DataType returns, StatementList statements, FunctionArgument[] arguments)
  {
    return new FunctionDeclaration()
    {
      Name = name,
      ReturnType = returns,
      Statements = statements,
      Arguments = arguments
    };
  }

  // if returning pointer type/object type without parameters
  public static FunctionDeclaration ClassReturner(string name, string returnTypeClassName, StatementList statements, FunctionArgument[] arguments)
  {
    return new FunctionDeclaration()
    {
      Name = name,
      ReturnType = DataType.CLASS_REF,
      ReturnTypeClassName = returnTypeClassName,
      Statements = statements,
      Arguments = arguments
    };
  }

  public string ToXML(int indentLevel)
  {
    string indent = new string('\t', indentLevel);
    string indent2 = new string('\t', indentLevel + 1);
    StringBuilder sb = new StringBuilder();
    sb.Append(indent);
    sb.AppendLine("<function>");

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

    sb.Append(Statements.ToXML(indentLevel + 1));

    sb.Append(indent);
    sb.AppendLine("</function>");

    return sb.ToString();
  }
}