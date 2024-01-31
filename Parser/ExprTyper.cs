using static PSI.Token.E;
namespace PSI;
class ExprTyper : Visitor<NType> {
   public override NType Visit (NLiteral literal)
      => literal.Type = literal.Value.Kind is STRING ? NType.String :
                        literal.Value.Kind is INTEGER ? NType.Int : NType.Real;

   public override NType Visit (NIdentifier identifier)
      => identifier.Type = NType.Int;

   public override NType Visit (NUnary unary)
      => unary.Type = unary.Expr.Accept (this);

   public override NType Visit (NBinary binary) {
      NType a = binary.Left.Accept (this), b = binary.Right.Accept (this);
      return binary.Type = (a, binary.Op.Kind, b) switch {
         (NType.Real or NType.Int, ADD or SUB or MUL or DIV, NType.Real or NType.Int) when a == b => a,
         (NType.Real or NType.Int, ADD or SUB or MUL or DIV, NType.Real or NType.Int) => NType.Real,
         (NType.String, ADD, _) => NType.String,
         (_, ADD, NType.String) => NType.String,
         (NType.Real or NType.Int, LT or GEQ or GT or LEQ or EQ or NEQ, NType.Real or NType.Int) => NType.Bool,
         (NType.String, LT or GT or EQ or NEQ, NType.String) => NType.Bool,
         _ => NType.Error
      };
   }
}