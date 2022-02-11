using System.Collections.Generic;
using System.Text;
using Jack_Compiler.Common;

public class VariableDeclarationList : List<VariableDeclaration>, ICanBeExpressedAsXML {

    public string ToXML(int indentLevel) {
        StringBuilder sb = new StringBuilder();
        string indent = new string('\t', indentLevel);
        sb.Append(indent);
        sb.AppendLine("<variable-declaration-list>");
        
        foreach (var item in this)
        {
            sb.Append(item.ToXML(indentLevel + 1));
        }
        
        sb.Append(indent);
        sb.AppendLine("</variable-declaration-list>");
        return sb.ToString();
    }
}

public static class ExtensionMethods {

  public static string ToXML<T>(this T[] that, string xmlName, int indentLevel) where T : ICanBeExpressedAsXML {
    StringBuilder sb = new StringBuilder();
    string indent = new string('\t', indentLevel);
    sb.Append(indent);
    sb.AppendLine($"<{xmlName}>");
    
    foreach (var item in that)
    {
      sb.Append(item.ToXML(indentLevel + 1));
    }
    
    sb.Append(indent);
    sb.AppendLine($"</{xmlName}>");
    return sb.ToString();
  }

  /*public static string ExpressAsXML<T>(this List<T> that, string xmlName, int indentLevel) where T : ICanBeExpressedAsXML {

    // className = ???

    StringBuilder sb = new StringBuilder();
    string indent = new string('\t', indentLevel);
    sb.Append(indent);
    sb.AppendLine($"<{xmlName}>");
    
    foreach (var item in that)
    {
        sb.Append(item.ToXML(indentLevel + 1));
    }
    
    sb.Append(indent);
    sb.AppendLine($"</{xmlName}>");
    return sb.ToString();
  }*/
}