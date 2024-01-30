namespace PSI;

static class Start {
   static void Main () {
      var parser = new Parser (new Tokenizer (Expr0));
      foreach (var expr in parser.Parse ()) {
         var node = expr;
         Console.WriteLine ($"Line: {node.Line}   EndColumn: {node.Column}");

         ExprTyper exprTyper = new ();
         Console.WriteLine ("Expression Type: " + node.Accept (exprTyper));

         var dict = new Dictionary<string, double> () { ["five"] = 5, ["two"] = 2 };
         double value = node.Accept (new ExprEvaluator (dict));
         Console.WriteLine ($"Value = {(node.Type == NType.Bool ? value == 1 : value)}");

         var sb = node.Accept (new ExprILGen ());
         Console.WriteLine ("\nGenerated code: ");
         Console.WriteLine (sb);

         ExprGrapher grapher = new ();
         node.Accept (grapher);
         Console.WriteLine ($"Grapher Code:\n{grapher.GrapherCode}");
      }
   }
   static string Expr0
      = "2 + 3\n(3 + 2.5) * 4 - 17 * -five * (two + 1 + 4 + 5)\n3 > 5\n3.5 * 2 > 10 + 3";
}
