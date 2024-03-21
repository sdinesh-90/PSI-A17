using PSI;

namespace Parser;
class ExprGrapher : Visitor<int> {
   StringBuilder mSB = new ();
   int mID = 0;
   public override int Visit (NLiteral literal) {
      string nodeId = $"id{++mID}";
      mSB.AppendLine ($"    {nodeId}[{literal.Value.Text}]");
      return mID;
   }

   public override int Visit (NIdentifier identifier) {
      string nodeId = $"id{++mID}";
      mSB.AppendLine ($"    {nodeId}[{identifier.Name.Text}]");
      return mID;
   }

   public override int Visit (NUnary unary) {
      var exprGraph = unary.Expr.Accept (this).ToString ();
      var unaryOp = unary.Op.Kind == Token.E.SUB ? "([-])" : $"([?])";
      string nodeId = $"id{++mID}";
      mSB.Append ($"    {nodeId}{unaryOp}");
      mSB.AppendLine ($"; {nodeId} --> id{exprGraph}");
      return mID;
   }

   public override int Visit (NBinary binary) {
      var leftGraph = binary.Left.Accept (this).ToString ();
      var rightGraph = binary.Right.Accept (this).ToString ();
      string nodeId = $"id{++mID}";
      mSB.Append ($"    {nodeId}([{binary.Op.Text}])");
      mSB.Append ($"; {nodeId} --> id{leftGraph}");
      mSB.AppendLine ($"; {nodeId} --> id{rightGraph}");
      return mID;
   }

   public void WriteToHtmlFile (string expression, string filePath) {
      using (StreamWriter writer = new (filePath)) {
         writer.WriteLine ("<!DOCTYPE html>");
         writer.WriteLine ("<head><meta charset=\"utf-8\"></head>");
         writer.WriteLine ("<body>");
         writer.WriteLine ($"  Graph of {expression}");
         writer.WriteLine ("  <pre class=\"mermaid\">");
         writer.WriteLine ("    graph TD");
         writer.Write (mSB.ToString ());
         writer.WriteLine ("  </pre>");
         writer.WriteLine ("  <script type=\"module\">");
         writer.WriteLine ("    import mermaid from 'https://cdn.jsdelivr.net/npm/mermaid@10/dist/mermaid.esm.min.mjs';");
         writer.WriteLine ("    mermaid.initialize({ startOnLoad: true });");
         writer.WriteLine ("</script>");
         writer.Write ("</body>");
      }
   }
}