namespace PSI;
using static Token.E;

// An expression evaluator, implementing using the visitor pattern
class ExprEvaluator : Visitor<double> {
   public ExprEvaluator (Dictionary<string, double> dict) => mDict = dict;
   Dictionary<string, double> mDict;

   public override double Visit (NLiteral literal)
      => double.Parse (literal.Value.Text);

   public override double Visit (NIdentifier identifier)
      => mDict[identifier.Name.Text];

   public override double Visit (NUnary unary) {
      double d = unary.Expr.Accept (this);
      if (unary.Op.Kind == SUB) d = -d;
      return d;
   }

   public override double Visit (NBinary binary) {
      double a = binary.Left.Accept (this), b = binary.Right.Accept (this);
      return binary.Op.Kind switch {
         ADD => a + b, SUB => a - b, MUL => a * b, DIV => a / b,
         LT => b.CompareTo(a), GT => a.CompareTo(b),
         LEQ => b.Equals (a) ? 1 : b.CompareTo (a), GEQ => a.Equals(b) ? 1: a.CompareTo(b),
         EQ => a.Equals(b) ? 1 : 0, NEQ => a.Equals (b) ? 0 : 1,
         _ => throw new NotImplementedException ()
      };
   }
}