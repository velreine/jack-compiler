using Jack_Compiler.Common;
using System.Text;

public class JackAST : ClassDeclaration {

  public JackAST(string n, VariableDeclarationList v, FunctionDeclaration[] f) : base(n, v, f) {}

  public string ToXML(int indentLevel = 0) {
    string s = new string('\t', indentLevel);
    StringBuilder sb = new StringBuilder();
    sb.AppendLine(s + "<jack-ast>");
    sb.Append(base.ToXML(indentLevel + 1));
    sb.AppendLine(s + "</jack-ast>");
    return sb.ToString();
  }
}