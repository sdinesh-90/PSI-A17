using static PSI.Token.E;
namespace PSI;
class ExprTyper : Visitor<NType> {
   public override NType Visit (NLiteral literal)
      => literal.Type = literal.Value.Kind switch {
         STRING => NType.String, INTEGER => NType.Int,
         REAL => NType.Real, _ => NType.Error
      };

   public override NType Visit (NIdentifier identifier)
      => identifier.Type = NType.Int;

   public override NType Visit (NUnary unary)
      => unary.Type = Visit(unary.Expr as dynamic);

   public override NType Visit (NBinary binary) {
      NType a = Visit(binary.Left as dynamic), b = Visit(binary.Right as dynamic);
      return binary.Type = (a, binary.Op.Kind, b) switch {
         (NType.Real or NType.Int, ADD or SUB or MUL or DIV, NType.Real or NType.Int) => a == b ? a : NType.Real, 
         (NType.String or _, ADD, _ or NType.String) => NType.String,
         (NType.Real or NType.Int, LT or GEQ or GT or LEQ or EQ or NEQ, NType.Real or NType.Int) => NType.Bool,
         (NType.String, LT or GT or EQ or NEQ, NType.String) => NType.Bool,
         _ => NType.Error
      };
   }
}