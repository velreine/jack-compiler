using Jack_Compiler.Common;
using System.Text;

public class ClassDeclaration : ICanBeExpressedAsXML
{
  public string Name { get; init; }
  public VariableDeclarationList VariableDeclarations { get; init; }
  public FunctionDeclaration[] SubroutineDeclarations { get; init; }

  // woo say the same thing 4 times wooooooooooooooooooooooo
  public ClassDeclaration(string name, VariableDeclarationList variableDeclarations, FunctionDeclaration[] subroutineDeclarations) : base()
  {
    this.Name = name;
    this.VariableDeclarations = variableDeclarations;
    this.SubroutineDeclarations = subroutineDeclarations;
  }

  public string ToXML(int indentLevel)
  {
    string indent = new string('\t', indentLevel);
    string indent2 = new string('\t', indentLevel + 1);
    StringBuilder sb = new StringBuilder();

    sb.AppendLine($"{indent}<class>");
    
    sb.AppendLine($"{indent2}<name>{Name}</name>");

    sb.Append(VariableDeclarations.ToXML(indentLevel + 1));

    sb.Append(SubroutineDeclarations.ToXML("subroutine-declarations", indentLevel + 1));

    sb.AppendLine($"{indent}</class>");

    return sb.ToString(); ;
  }
}