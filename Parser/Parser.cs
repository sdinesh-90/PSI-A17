namespace PSI;
using static Token.E;

class Parser {
   public Parser (Tokenizer tokenizer)
      => mToken = mPrevious = (mTokenizer = tokenizer).Next ();
   Tokenizer mTokenizer;
   Token mToken, mPrevious;

   public NExpr Parse () => Expression ();

   // Implementation --------------------------------------
   // expression = equality .
   NExpr Expression ()
      => Equality ();

   // equality = comparison [ ( "=" | "<>") comparison ] .
   NExpr Equality () {
      var expr = Comparison ();
      if (Match (EQ, NEQ))
         expr = new NBinary (expr, mPrevious, Comparison ());
      return expr;
   }

   // comparison = term [ ("<" | "<=" | ">" | ">=") term ] .
   NExpr Comparison () {
      var expr = Term ();
      if (Match (LT, GT, LEQ, GEQ))
         expr = new NBinary (expr, mPrevious, Term ());
      return expr;
   }

   // term = factor { ("+" | "-") factor } .
   NExpr Term () {
      var expr = Factor ();
      while (Match (ADD, SUB)) {
         var op = mPrevious;
         expr = new NBinary (expr, op, Factor ());
      }
      return expr;
   }

   // factor = unary { ("*" | "/") unary } .
   NExpr Factor () {
      var expr = Unary ();
      while (Match (MUL, DIV)) {
         var op = mPrevious;
         expr = new NBinary (expr, op, Unary ());
      }
      return expr;
   }

   // unary = ( "-" | "+" ) unary | primary .
   NExpr Unary () {
      if (Match (ADD, SUB))
         return new NUnary (mPrevious, Unary ());
      return Primary ();
   }

   // primary = IDENTIFIER | INTEGER | REAL | STRING | "(" expression ")" .
   NExpr Primary () {
      if (Match (IDENT)) return new NIdentifier (mPrevious);
      if (Match (INTEGER, REAL, STRING)) return new NLiteral (mPrevious);
      Expect (OPEN, "Expecting identifier or literal");
      var expr = Expression ();
      Expect (CLOSE, "Expecting ')'");
      return expr;
   }

   // Helpers ---------------------------------------------
   // Expect to find a particular token
   void Expect (Token.E kind, string message) {
      if (!Match (kind)) throw new Exception (message);
   }

   // Match and consume a token on match
   bool Match (params Token.E[] kinds) {
      if (kinds.Contains (mToken.Kind)) {
         mPrevious = mToken;
         mToken = mTokenizer.Next ();
         return true;
      }
      return false;
   }
}