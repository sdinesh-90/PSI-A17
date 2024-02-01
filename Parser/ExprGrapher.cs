namespace PSI;
class ExprGrapher : Visitor<int> {
   StringBuilder mSB = new ();
   int mID = 1;

   public override int Visit (NLiteral literal) {
      mSB.AppendLine ($"Id{mID}[{literal.Value.Text}\\n{literal.Type}]");
      return mID++;
   }

   public override int Visit (NIdentifier identifier) {
      mSB.AppendLine ($"Id{mID}[{identifier.Name.Text}\\n{identifier.Type}]");
      return mID++;
   }

   public override int Visit (NUnary unary) {
      int down = unary.Expr.Accept (this);
      mSB.AppendLine ($"Id{mID}([{unary.Op.Text}\\n{unary.Type}]); Id{mID} --> Id{down}");
      return mID++;
   }

   public override int Visit (NBinary binary) {
      int left = binary.Left.Accept (this);
      int right = binary.Right.Accept (this);
      mSB.AppendLine ($"Id{mID}([{binary.Op.Text}\\n{binary.Type}]); Id{mID} --> Id{left}; Id{mID} --> Id{right}");
      return mID++;
   }

   public void Save (string expr, string path) {
      string code = $$"""
            <!DOCTYPE html>
            <head><meta charset="utf-8"></head>
            <body>
              Graph of {{expr}}
              <pre class="mermaid">
                graph TD
                {{GrapherCode}}
              </pre> 
              <script type="module">
                import mermaid from 'https://cdn.jsdelivr.net/npm/mermaid@10/dist/mermaid.esm.min.mjs';
                mermaid.initialize({ startOnLoad: true });
              </script>  
            </body>
            """;
      File.WriteAllText (path, code);
   }

   public StringBuilder GrapherCode => mSB;
}