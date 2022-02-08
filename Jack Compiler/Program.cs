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
                var fileName = Console.ReadLine();
                var fqFileName = @"./../../../Tests/" + fileName + ".xml";
                var file = File.ReadAllText(@"./../../../Tests/" + fileName + ".jack");

                // Test cleanup of file.
                //file = CleanInputContent(file);

                var outFile = fqFileName;
                //System.Console.WriteLine(file + "\n");

                TokenList tokens = (TokenList)(new Lexer(file)).LexTokens();

                File.WriteAllText(outFile, tokens.ToXML());
            }
 
            // Cleans the content of the file making it easier for the Lexer to "lex" it.
         /* private static string CleanInputContent(string fileContent)
            {
                // Replace new lines with a space.
                fileContent = fileContent.Replace(Environment.NewLine, " ");
                
                Console.WriteLine(fileContent);
                
                // Put a space after identifiers.
                fileContent = Regex.Replace(fileContent, "([A-Za-z0-9_]+)", m => " " + m.Groups[1].Value + " ");

                fileContent = Regex.Replace(fileContent, @"([()\[\]{}<>\-+,;*./&|=~])", m => " " + m.Groups[1].Value + " ");
                
                Console.WriteLine(fileContent);
                // Then remove duplicate spaces.
                fileContent = Regex.Replace(fileContent, "[ ]+", " ");
                
                Console.WriteLine(fileContent);
                return fileContent.Trim();
            }*/
    }
}