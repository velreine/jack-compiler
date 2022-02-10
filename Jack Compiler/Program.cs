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
            // "quick-math",
            // "sommer",
            // "hello-world-easy",
            // "hello-world-hard",//NoobSlayer69   thanks
            // "expr-test",
            //"statement",
            "wrong",
            BadJokes.GetJoke(),
        };

      foreach (var shit in fileNames)
      {
        if (shit == fileNames[fileNames.Length - 1])
        {
          System.Console.WriteLine(shit);
          continue;
        }
        var fqFileName__ = @"./../../../Tests/" + shit + ".xml";
        var fqFileNameAst = @"./../../../Tests/" + shit + ".ast.xml";
        var file__ = File.ReadAllText(@"./../../../Tests/" + shit + ".jack");
        var tokens = (new Lexer(file__)).LexTokens();
        File.WriteAllText(fqFileName__, tokens.ToXML(0));
        var jackAst = (new Parser(tokens)).ParseTokens();
        File.WriteAllText(fqFileNameAst, jackAst.ToXML(0));
      }

      // var fileName = Console.ReadLine();
      // var fqFileName = @"./../../../Tests/" + fileName + ".xml";
      // var file = File.ReadAllText(@"./../../../Tests/" + fileName + ".jack");

      // var outFile = fqFileName;

      // TokenList tokens = (new Lexer(file)).LexTokens();
      // File.WriteAllText(outFile, tokens.ToXML());

      // var parser = new Parser(tokens);

      // var jackAst = parser.ParseTokens();

      int debug = 2;

      // https://www.youtube.com/watch?v=p7YXXieghto

      //  █▀▀▀▀▀█ ▀▄▄▀▀██   █▀▀▀▀▀█
      //  █ ███ █   ▀ ██▀▀▄ █ ███ █
      //  █ ▀▀▀ █  ▀▄▀▄▄█ ▀ █ ▀▀▀ █
      //  ▀▀▀▀▀▀▀ █ █▄▀▄▀ █ ▀▀▀▀▀▀▀
      //  ▀█▄▀█ ▀▄ █▀▀▄▄▄█▀▄▀▄▄▄▄▄▀
      //    █▄██▀▄▄▄██▄▀ █▄█▀█▀█▄▄█
      //  ▄█▀ ▀▄▀ ██ ▀▀▄▄█▄ ▀▀▄  ▄▀
      //  █▀▀▀ █▀▄█▀▄█▀▀██▀▀█▄▀██▀█
      //  ▀ ▀▀ ▀▀ ▄█▀▄▄█ ▀█▀▀▀█ ██ 
      //  █▀▀▀▀▀█  ▀█▀▄▄▀▄█ ▀ █  ▄▀
      //  █ ███ █ █ ▄ ▀▄▄▀███▀▀  ██
      //  █ ▀▀▀ █ ▄█▄ ▀▀▄▄ ▀▄▄█▀███
      //  ▀▀▀▀▀▀▀ ▀▀  ▀▀▀ ▀ ▀  ▀  ▀
    }
  }
}