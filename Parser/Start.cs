using System.Diagnostics;
namespace PSI;

static class Start {
   static void Main () {
      var parser = new Parser (new Tokenizer (Expr0));
      int count = 0;
      string[] exprList = Expr0.Split ('\n');
      foreach (var expr in parser.Parse ()) {
         var node = expr;

         ExprTyper exprTyper = new ();
         Console.WriteLine ("Expression Type: " + exprTyper.Visit (node as dynamic));

         var dict = new Dictionary<string, double> () { ["five"] = 5, ["two"] = 2 };
         double value = node.Accept (new ExprEvaluator (dict));
         Console.WriteLine ($"Value = {(node.Type == NType.Bool ? value == 1 : value)}");

         var sb = node.Accept (new ExprILGen ());
         Console.WriteLine ("\nGenerated code: ");
         Console.WriteLine (sb);

         ExprGrapher grapher = new ();
         node.Accept (grapher);
         grapher.Save (exprList[count++], "../bin/GrapherOutput.html");
         var pi = new ProcessStartInfo ("GrapherOutput.html") { UseShellExecute = true };
         Process.Start (pi);
         Console.Write ("\nPress any key...\n"); Console.ReadKey (true);
      }
   }
   static string Expr0
      = "2 + 3\n(3 + 2.5) * 4 - 17 * -five * (two + 1 + 4 + 5)\n3 > 5\n3.5 * 2 > 10 + 3";
}
