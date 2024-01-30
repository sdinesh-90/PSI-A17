namespace PSI;
using static Token.E;

// Represents a PSI language Token
class Token {
   public Token (E kind, string text, int line, int col) => (Kind, Text, Line, Column) = (kind, text, line, col);
   public E Kind { get; }
   public string Text { get; }
   public int Line { get; }
   public int Column { get; }

   // The various types of token
   public enum E {
      // Keywords
      PROGRAM, VAR, IF, THEN, WHILE,
      DO, BEGIN, END, PRINT, _ENDKEYWORDS,
      // Operators
      ADD, SUB, MUL, DIV, NEQ,
      LEQ, GEQ, EQ, LT, GT,
      ASSIGN, _ENDOPERATORS,
      // Punctuation
      SEMI, PERIOD, COMMA, OPEN, CLOSE,
      COLON, _ENDPUNCTUATION,
      // Others
      IDENT, INTEGER, REAL, BOOLEAN, STRING, EOF, ERROR, NL
   }

   // Print a Token
   public override string ToString () {
      if (Kind is EOF or ERROR) return Kind.ToString ();
      if (Kind < _ENDKEYWORDS) return Kind.ToString ().ToLower ();
      if (Kind == STRING) return $"\"{Text}\"";
      if (Kind == INTEGER) return $"int({Text})";
      if (Kind == REAL) return $"real({Text})";
      return $"{Kind}:{Text}";
   }

   // Helper used by the parser (maps operator sequences to E values)
   public static List<(E Kind, string Text)> Match = new () {
      (NEQ, "<>"), (LEQ, "<="), (GEQ, ">="), (ASSIGN, ":="), (ADD, "+"),
      (SUB, "-"), (MUL, "*"), (DIV, "/"), (EQ, "="), (LT, "<"),
      (LEQ, "<="), (GT, ">"), (SEMI, ";"), (PERIOD, "."), (COMMA, ","),
      (OPEN, "("), (CLOSE, ")"), (COLON, ":"), (NL, "\n")
   };
}