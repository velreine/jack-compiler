using System.Collections.Generic;
using System.Text;
using Jack_Compiler.Common;

public class StatementList : List<Statement>, ICanBeExpressedAsXML {

    public string ToXML(int indentLevel) {
        StringBuilder sb = new StringBuilder();
        string indent = new string('\t', indentLevel);
        sb.Append(indent);
        sb.AppendLine("<statement-list>");
        
        foreach (var item in this)
        {
            sb.Append(item.ToXML(indentLevel + 1));
        }
        
        sb.Append(indent);
        sb.AppendLine("</statement-list>");
        return sb.ToString();
    }
}