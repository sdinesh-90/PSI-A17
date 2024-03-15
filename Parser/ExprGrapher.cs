using PSI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Parser {
   class ExprGrapher : Visitor<int> {
      StringBuilder mSb = new ();
      int mId = 0;
      public override int Visit (NLiteral literal) {
         mSb.AppendLine ($"id{++mId}[{literal.Value.Text}]");
         return mId;
      }

      public override int Visit (NIdentifier ident) {
         mSb.AppendLine ($"id{++mId}[{ident.Name.Text}]");
         return mId;
      }

      public override int Visit (NUnary unary) {
         int d = unary.Expr.Accept (this);
         mSb.AppendLine ($"id{++mId}([{unary.Op.Text}]); id{mId} --> id{d}");
         return mId;
      }

      public override int Visit (NBinary binary) {
         int a = binary.Left.Accept (this), b = binary.Right.Accept (this);
         mSb.AppendLine ($"id{++mId}([{binary.Op.Text}]); id{mId} --> id{a}; id{mId} --> id{b}");
         return mId;
      }
      public void WriteToFile (string exp) {
         string str = $$"""
            <!DOCTYPE html>
            <head><meta charset="utf-8"></head>
            <body>
              Graph of {{exp}}
              <pre class="mermaid">
                graph TD
                {{mSb}}
              </pre>
              <script type="module">
                import mermaid from 'https://cdn.jsdelivr.net/npm/mermaid@10/dist/mermaid.esm.min.mjs';
                mermaid.initialize({startOnLoad: true});
              </script>
            </body>
            """;
         File.WriteAllText ("C:/etc/Text.html", str);
      }
   }
}