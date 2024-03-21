using Parser;
namespace PSI;
static class Start {
   static void Main () {
      var parser = new Parser (new Tokenizer (Expr0));
      var node = parser.Parse ();

      var dict = new Dictionary<string, int> () { ["five"] = 5, ["two"] = 2 };
      int value = node.Accept (new ExprEvaluator (dict));
      Console.WriteLine ($"Value = {value}");

      var sb = node.Accept (new ExprILGen ());
      Console.WriteLine ("\nGenerated code: "); 
      Console.WriteLine (sb);

      ExprGrapher exprGrapher = new();
      _ = node.Accept (exprGrapher);
      string filePath = "../Parser/Data/output.html";
      exprGrapher.WriteToHtmlFile (Expr0, filePath);
   }

   static string Expr0
      = "(3 + 2) * 40 - 17 * -five * (two + 1 + 4 + 5)";
}
