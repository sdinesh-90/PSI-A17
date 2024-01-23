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

   public override NType Visit (NBinary binary)
      => binary.Type = (binary.Left.Accept (this), binary.Op.Kind, binary.Right.Accept (this)) switch {
         (NType.Real, ADD, NType.Real) => NType.Real,
         (NType.Real, SUB, NType.Real) => NType.Real,
         (NType.Real, MUL, NType.Real) => NType.Real,
         (NType.Real, DIV, NType.Real) => NType.Real,
         (NType.Int, ADD, NType.Int) => NType.Int,
         (NType.Int, SUB, NType.Int) => NType.Int,
         (NType.Int, MUL, NType.Int) => NType.Int,
         (NType.Int, DIV, NType.Int) => NType.Real,
         (NType.Int, SUB, NType.Real) => NType.Real,
         (NType.Real, SUB, NType.Int) => NType.Real,
         (NType.Int, ADD, NType.Real) => NType.Real,
         (NType.Real, ADD, NType.Int) => NType.Real,
         (NType.Int, MUL, NType.Real) => NType.Real,
         (NType.Real, MUL, NType.Int) => NType.Real,
         (NType.Int, DIV, NType.Real) => NType.Real,
         (NType.Real, DIV, NType.Int) => NType.Real,
         (NType.String, ADD, NType.String) => NType.String,
         _ => NType.Error
      };
}