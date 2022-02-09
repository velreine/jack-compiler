using System;
using System.IO;
using Jack_Compiler.Tokens;
using System.Text;
using System.Text.RegularExpressions;

namespace Jack_Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Give me a file name.");
            var fileNames = new string[] {
                "quick-math",
                "sommer",
                "hello-world",
            };
            var fileName = Console.ReadLine();
            var fqFileName = @"./../../../Tests/" + fileName + ".xml";
            var file = File.ReadAllText(@"./../../../Tests/" + fileName + ".jack");

            // Test cleanup of file.
            //file = CleanInputContent(file);

            var outFile = fqFileName;
            //System.Console.WriteLine(file + "\n");

            TokenList tokens = (new Lexer(file)).LexTokens();

            File.WriteAllText(outFile, tokens.ToXML());
        }
    }
}