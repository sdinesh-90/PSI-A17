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
      int opID = mID;
      mSB.AppendLine ($"Id{mID++}([{unary.Op.Text}\\n" +
         $"{(unary.Op.Text == "-" ? "NEG" : "POS")}]); Id{opID} --> Id{down}");
      return opID;
   }

   public override int Visit (NBinary binary) {
      int left = binary.Left.Accept (this);
      int right = binary.Right.Accept (this);
      int opID = mID;
      mSB.AppendLine ($"Id{mID++}([{binary.Op.Text}\\n" +
         $"{binary.Op.Kind}]); Id{opID} --> Id{left}; Id{opID} --> Id{right}");
      return opID;
   }

   public StringBuilder GrapherCode => mSB;
}