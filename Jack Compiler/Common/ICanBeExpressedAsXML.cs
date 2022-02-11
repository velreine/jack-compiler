namespace Jack_Compiler.Common {
    public interface ICanBeExpressedAsXML {
        public abstract string ToXML(int indentLevel);
        // TODO: FromXML mayhaps
    }
}