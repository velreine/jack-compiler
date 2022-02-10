using System.Collections.Generic;
using System.Text;
using System.IO;
using Jack_Compiler.Common;

namespace Jack_Compiler.Tokens {
    public class TokenList : List<Token>, ICanBeExpressedAsXML {
        public string ToXML(int indentLevel) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<token-list>");

            foreach (var token in this)
            {
                sb.AppendLine(token.ToXML(indentLevel + 1));
            }

            sb.AppendLine("</token-list>");
            return sb.ToString();
        }
    }
}