public class ParenthesizedExpression : Expression
{
    public Expression InnerExpression { get; }

    public ParenthesizedExpression(CodeLocation location, Expression inner)
        : base(location)
    {
        InnerExpression = inner;
    }

    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        // La validación semántica se delega a la expresión interna
        return InnerExpression.checksemantic(context, errors);
    }

    public override void Evaluate()
    {
        InnerExpression.Evaluate();
        Value = InnerExpression.Value;
    }
    public override ExpressionType Type { get; set; }
    public override object? Value { get; set; }
}