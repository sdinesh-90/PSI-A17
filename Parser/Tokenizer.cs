namespace PSI;
using static Token.E;

// Converts a stream of text to PSI Tokens
class Tokenizer {
   public Tokenizer (string text) => mText = text + " ";
   readonly string mText;
   int mN, mLine = 1, mLineStart = -1;

   // Returns the next token (returns EOF token if no more are left)
   public Token Next () {
      for (; ; ) {
         if (mN >= mText.Length) return MakeToken (EOF, "");
         char ch = mText[mN];
         if (ch == '\n') { mLine++; mLineStart = mN++; return MakeToken (NL, "\n"); }
         if (char.IsWhiteSpace (ch)) { mN++; continue; }
         if (char.IsLetter (ch)) return IdentifierOrKeyword ();
         if (char.IsDigit (ch)) return Number ();
         if (ch == '"') return String ();
         return PunctuationOrOperator ();
      }
   }

   // Implementation ------------------------------------------------
   // If we see a numeric digit, construct a number token
   Token Number () {
      var m = sExpr.Match (mText[mN..]);
      if (m.Success) {
         string text = mText[mN..(mN + m.Length)];
         mN += m.Length;
         if (text.Contains ('.') || text.Contains ('e') || text.Contains ('E'))
            return MakeToken (REAL, text);
         else
            return MakeToken (INTEGER, text);
      }
      return MakeToken (ERROR, "");
   }
   static Regex sExpr = new (@"^[+-]?\d+(\.\d+)?([Ee][+-]?\d+)?", RegexOptions.Compiled | RegexOptions.CultureInvariant);

   // If we see an open " then we construct a string token
   Token String () {
      int start = mN++;
      while (mN < mText.Length && mText[mN++] != '"') { }
      return MakeToken (STRING, mText[(start + 1)..(mN - 1)]);
   }

   // If we see an alpha character, construct an identifier or keyword
   Token IdentifierOrKeyword () {
      int start = mN;
      while (char.IsLetterOrDigit (mText[++mN])) { }
      string s = mText[start..mN];
      if (s == s.ToLower () && Enum.TryParse (s, true, out Token.E kind) && kind < _ENDKEYWORDS)
         return MakeToken (kind, "");
      return s switch { "true" or "false" => MakeToken (BOOLEAN, s), _ => MakeToken (IDENT, s) };
   }

   // Construct a punctuation or an operator token
   Token PunctuationOrOperator () {
      foreach (var (kind, text) in Token.Match) {
         int n = text.Length;
         if (mN + n < mText.Length && text == mText[mN..(mN + n)]) {
            mN += n;
            return MakeToken (kind, text);
         }
      }
      return MakeToken (ERROR, "");
   }

   Token MakeToken (Token.E kind, string text)
      => new (kind, text, mLine, mN - mLineStart);
}