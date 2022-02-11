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