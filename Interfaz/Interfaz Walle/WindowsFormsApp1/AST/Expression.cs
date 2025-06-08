
namespace WindowsFormsApp1
{
    public abstract class Expression : AST
    {
        public abstract void Evaluate();

        public abstract ExpressionType Type { get; set; }

        public abstract object Value { get; set; }

        public Expression(CodeLocation location) : base(location) { }

    }
}