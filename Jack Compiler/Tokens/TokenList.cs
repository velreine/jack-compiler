using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Jack_Compiler.Tokens {
    public class TokenList : List<Token>, ICanBeExpressedAsXML {
        public string ToXML() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<token_list>");

            foreach (var token in this)
            {
                sb.AppendLine("\t" + token.ToXML());
                // Console.WriteLine(token.ToXML());
            }

            sb.AppendLine("</token_list>");
            return sb.ToString();
        }
    }
}