namespace Jack_Compiler.Common {
    public interface ICanBeExpressedAsXML {
        public abstract string ToXML(int indentLevel = 0);
        // TODO: FromXML mayhaps
    }
}