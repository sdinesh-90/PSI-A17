namespace PSI;
class ExprGrapher : Visitor<StringBuilder> {
   StringBuilder mSB = new ();
   int id = 1;

   public override StringBuilder Visit (NLiteral literal)
      => mSB.AppendLine ($"Id{id++}[{literal.Value.Text}]");

   public override StringBuilder Visit (NIdentifier identifier)
      => mSB.AppendLine ($"Id{id++}[{identifier.Name.Text}]");

   public override StringBuilder Visit (NUnary unary) {
      int down = id;
      bool fl = BranchOnLeft;
      BranchOnLeft = true;
      unary.Expr.Accept (this);
      int opID = id;
      mSB.AppendLine ($"Id{id++}([{unary.Op.Text}]); Id{opID} --> Id{down}");
      if (fl) left = opID; else right = opID;
      return mSB;
   }

   // Local variables - state of current expression
   // Global variables - state of last evaluated expression
   public override StringBuilder Visit (NBinary binary) {
      bool branchOnLeft = BranchOnLeft;
      this.left = id;
      BranchOnLeft = true;
      binary.Left.Accept (this);
      int left = this.left;
      right = id;
      BranchOnLeft = false;
      binary.Right.Accept (this);
      int opID = id;
      mSB.AppendLine ($"Id{id++}([{binary.Op.Text}]); Id{opID} --> Id{left}; Id{opID} --> Id{right}");
      if (branchOnLeft) this.left = opID; else right = opID;
      return mSB;
   }
   int left, right;
   bool BranchOnLeft;
}