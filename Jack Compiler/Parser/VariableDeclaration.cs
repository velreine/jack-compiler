using Jack_Compiler.Common;
using System.Text;

public class VariableDeclaration : ICanBeExpressedAsXML
{

  public VariableType VariableType { get; init; }
  public string Name { get; init; }
  public DataType Type { get; init; }
  private string _typeClassName;
  public string TypeClassName
  {
    get
    {

      if (this.Type != DataType.CLASS_REF)
      {
        throw new System.Exception("Tried to access TypeClassName even though this variable's type is not CLASS_REF");
      }

      return _typeClassName;
    }
    init { _typeClassName = value; }
  }

  // Gør Camilla glad.
  private VariableDeclaration() : base /*d*/ ()
  {
    //based?
  }

  public static VariableDeclaration Primitive(string name, DataType typePrimitive, VariableType varType)
  {
    return new VariableDeclaration
    {
      Type = typePrimitive,//trailing coma
      VariableType = varType/*trailing comma*/,
      Name = name,
    };
  }

  public static VariableDeclaration ClassVar(string name, string typeClassName, VariableType variableType)
  {
    return new VariableDeclaration()
    {
      Type = DataType.CLASS_REF,
      TypeClassName = typeClassName,
      VariableType = variableType,
    };
  }

  // so we back in the mine, got our dicks swinging from side to side, side-side to side.
  // This task, a grueling one, hope to find some pussy tonight, night night, pussy tonight.
  public string ToXML(int indentLevel)
  {
    string indent = new string('\t', indentLevel);
    string indent2 = new string('\t', indentLevel + 1);
    StringBuilder sb = new StringBuilder();
    sb.Append(indent);
    sb.AppendLine("<variable>");

    if (Type == DataType.CLASS_REF)
    {
      sb.AppendLine(indent2 + $"<type-name>{TypeClassName}</type-name>");
    }
    else
    {
      sb.AppendLine(indent2 + $"<type-name>{Type.ToString()}</type-name>");
    }

    sb.AppendLine(indent2 + $"<var-name>{Name}</var-name>");

    sb.Append(indent);
    sb.AppendLine("</variable>");

    return sb.ToString(); ;
  }
}