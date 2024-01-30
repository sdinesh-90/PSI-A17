namespace PSI;
using static Token.E;

class Parser {
   public Parser (Tokenizer tokenizer)
      => mToken = mPrevious = (mTokenizer = tokenizer).Next ();
   Tokenizer mTokenizer;
   Token mToken, mPrevious;

   // expression { ("\n") expression } .
   public List<NExpr> Parse () {
      mExpr.Add (Expression ());
      while (Match(NL))
         mExpr.Add (Expression ());
      return mExpr;
   }
   List<NExpr> mExpr = new ();

   // Implementation --------------------------------------
   // expression = equality .
   NExpr Expression ()
      => Equality ();

   // equality = comparison [ ( "=" | "<>") comparison ] .
   NExpr Equality () {
      var expr = Comparison ();
      if (Match (EQ, NEQ))
         expr = new NBinary (expr, mPrevious, Comparison ()) { Line = mPrevious.Line, Column = mPrevious.Column };
      return expr;
   }

   // comparison = term [ ("<" | "<=" | ">" | ">=") term ] .
   NExpr Comparison () {
      var expr = Term ();
      if (Match (LT, GT, LEQ, GEQ))
         expr = new NBinary (expr, mPrevious, Term ()) { Line = mPrevious.Line, Column = mPrevious.Column };
      return expr;
   }

   // term = factor { ("+" | "-") factor } .
   NExpr Term () {
      var expr = Factor ();
      while (Match (ADD, SUB)) {
         var op = mPrevious;
         expr = new NBinary (expr, op, Factor ()) { Line = mPrevious.Line, Column = mPrevious.Column };
      }
      return expr;
   }

   // factor = unary { ("*" | "/") unary } .
   NExpr Factor () {
      var expr = Unary ();
      while (Match (MUL, DIV)) {
         var op = mPrevious;
         expr = new NBinary (expr, op, Unary ()) { Line = mPrevious.Line, Column = mPrevious.Column };
      }
      return expr;
   }

   // unary = ( "-" | "+" ) unary | primary .
   NExpr Unary () {
      if (Match (ADD, SUB))
         return new NUnary (mPrevious, Unary ()) { Line = mPrevious.Line, Column = mPrevious.Column };
      return Primary ();
   }

   // primary = IDENTIFIER | INTEGER | REAL | STRING | "(" expression ")" .
   NExpr Primary () {
      if (Match (IDENT)) return new NIdentifier (mPrevious) { Line = mPrevious.Line, Column = mPrevious.Column };
      if (Match (INTEGER, REAL, STRING, NL)) return new NLiteral (mPrevious) { Line = mPrevious.Line, Column = mPrevious.Column };
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