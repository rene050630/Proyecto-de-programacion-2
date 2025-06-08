namespace WindowsFormsApp1
{
    public abstract class BinaryExpression : Expression
    {
        public Expression Left;
        public Expression Right;
        public BinaryExpression(CodeLocation location) : base(location) { }
    }
}