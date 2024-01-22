namespace PSI;
class ExprGrapher : Visitor<int> {
   StringBuilder mSB = new ();
   int mID = 1;

   public override int Visit (NLiteral literal) {
      mSB.AppendLine ($"Id{mID}[{literal.Value.Text}]");
      return mID++;
   }

   public override int Visit (NIdentifier identifier) {
      mSB.AppendLine ($"Id{mID}[{identifier.Name.Text}]");
      return mID++;
   }

   public override int Visit (NUnary unary) {
      int down = unary.Expr.Accept (this);
      int opID = mID;
      mSB.AppendLine ($"Id{mID++}([{unary.Op.Text}]); Id{opID} --> Id{down}");
      return opID;
   }

   public override int Visit (NBinary binary) {
      int left = binary.Left.Accept (this);
      int right = binary.Right.Accept (this);
      int opID = mID;
      mSB.AppendLine ($"Id{mID++}([{binary.Op.Text}]); Id{opID} --> Id{left}; Id{opID} --> Id{right}");
      return opID;
   }

   public StringBuilder GrapherCode => mSB;
}