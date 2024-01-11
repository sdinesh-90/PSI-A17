namespace PSI;

// Base class for all syntax-tree Nodes
abstract class Node {
}

// The data-type at any NExpr node
enum NType { Unknown, Int, Real, Bool, String, Error };

// Base class for all expression nodes
abstract class NExpr : Node {
   public NType Type { get; set; }
   abstract public T Accept<T> (Visitor<T> visitor);
}

// Represents a binary operation node
class NBinary : NExpr {
   public NBinary (NExpr left, Token op, NExpr right) => (Left, Op, Right) = (left, op, right);
   public NExpr Left { get; }
   public Token Op { get; }
   public NExpr Right { get; }

   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// Represents a unary operation node
class NUnary : NExpr {
   public NUnary (Token op, NExpr expr) => (Op, Expr) = (op, expr);
   public Token Op { get; }
   public NExpr Expr { get; }

   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// An identifier node
class NIdentifier : NExpr {
   public NIdentifier (Token name) => Name = name;
   public Token Name { get; }

   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// A number or string literal node
class NLiteral : NExpr {
   public NLiteral (Token value) => Value = value;
   public Token Value { get; }

   public override T Accept<T> (Visitor<T> visitor) => visitor.Visit (this);
}

// The ExprVisitor interface
abstract class Visitor<T> {
   abstract public T Visit (NLiteral literal);
   abstract public T Visit (NIdentifier identifier);
   abstract public T Visit (NUnary unary);
   abstract public T Visit (NBinary binary);
}