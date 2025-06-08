namespace WindowsFormsApp1
{
    public abstract class Statement : AST
    {
        public Statement(CodeLocation location) : base(location) { }
        public abstract void Execute();
    }
}