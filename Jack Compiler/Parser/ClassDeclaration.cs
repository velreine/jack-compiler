using Jack_Compiler.Common;
using System.Text;
// using Family.Your.Mom.Kan.Ikke.Stave.Til.Det;
// using Kentuky.Family.Incest;

public class ClassDeclaration : ICanBeExpressedAsXML
{
  // Static variables ?
  public string Name { get; init; }
  public VariableDeclarationList VariableDeclarations { get; init; }
  public FunctionDeclaration Constructor { get; init; }
  public FunctionDeclaration[] FunctionDeclarations { get; init; }

  // gør Camilla super glad
  public ClassDeclaration() : base()
  {
    // Constructor skal være her ellers bliver Camilla sur.
    // Camilla when no constructor:
    // https://www.google.com/search?q=angry+spongebob+ip+address&tbm=isch&ved=2ahUKEwj4z42t6fT1AhVCXcAKHS1VB9wQ2-cCegQIABAA&oq=angry+spongebob+ip+address&gs_lcp=CgNpbWcQAzoHCCMQ7wMQJzoFCAAQgAQ6BAgAEENQtgJY7BVg1RZoAHAAeACAAUqIAeIFkgECMTKYAQCgAQGqAQtnd3Mtd2l6LWltZ8ABAQ&sclient=img&ei=7toEYvigJcK6gQatqp3gDQ&bih=969&biw=1920&rlz=1C1GCEA_enDK906DK906#imgrc=epHLPTLgOCa6aM
  }


  public string ToXML(int indentLevel)
  {
    StringBuilder sb = new StringBuilder();











    return sb.ToString(); ;
  }

}