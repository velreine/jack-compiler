using System;
using Jack_Compiler.Common;
using System.Text;

public enum ExprType {
    TERM,   // expr = term
    TERM_OP_TERM,     // expr = term op term
}

public class Expr : ICanBeExpressedAsXML {
    public ExprType Type { get; init; }
    public Term Left { get; init; }
    public Operator Op { get; init; }
    public Term Right { get; init; }  

    public Expr(Term left) {
        if(left == null) {
            throw new Exception("The left expression must never be NULL");
        }
        this.Type = ExprType.TERM;
        this.Left = left;
    }
    public Expr(Term left, Operator op, Term right) {  

        if (left is null || op is null || right is null) {
            throw new Exception("NULL in Expr constructor arguments");
        }

        this.Left = left;
        this.Op = op;
        this.Right = right;
        this.Type = ExprType.TERM_OP_TERM;
    }

    public string ToXML(int indentLevel) {
        string indentComp = new string('\t', indentLevel);
        StringBuilder sb = new StringBuilder();
        sb.Append(indentComp);
        sb.AppendLine("<expression>");
        if (Type == ExprType.TERM) {
            sb.AppendLine(Left.ToXML(indentLevel + 1));
        } else {
            sb.AppendLine(Left.ToXML(indentLevel + 1));
            sb.AppendLine(Op.ToXML(indentLevel + 1));
            sb.AppendLine(Right.ToXML(indentLevel + 1));
        }
        sb.Append(indentComp);
        sb.AppendLine("</expression>");
        return sb.ToString();
    }
}

// https://www.youtube.com/watch?v=p7YXXieghto