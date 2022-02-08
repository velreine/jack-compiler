using System;
using System.Xml;
using System.IO;
using Jack_Compiler.Tokens;
using System.Text;

namespace Jack_Compiler
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Give me a file name.");
      var fileName = Console.ReadLine();
      var fqFileName = @"./../../../Tests/" + fileName + ".xml";
      var file = File.ReadAllText(@"./../../../Tests/" + fileName + ".jack");
      var outFile = fqFileName;
      System.Console.WriteLine(file + "\n");

      var tokens = (new Lexer(file)).LexTokens();

      StringBuilder sb = new StringBuilder();
      sb.AppendLine("<token_list>");

      foreach (var token in tokens)
      {
        sb.AppendLine("\t" + token.ToXML());
        // Console.WriteLine(token.ToXML());
      }

      sb.AppendLine("</token_list>");
      File.WriteAllText(outFile, sb.ToString());
    }
  }
}
