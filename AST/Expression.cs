
public abstract class Expression : AST
{
    public abstract void Evaluate(ExecutionContext context);

    public abstract ExpressionType Type { get; set; }

    public abstract object? Value { get; set; }

    public Expression(CodeLocation location) : base (location) { }

}