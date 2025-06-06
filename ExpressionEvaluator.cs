public class ExpressionEvaluator : Statement
{
    Expression Expression;
    public ExpressionEvaluator(Expression expression, CodeLocation location) : base(location)
    {
        Expression = expression;
    }
    public override void Execute()
    {
        Expression.Evaluate();
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        bool valid = Expression.checksemantic(context, errors);
        if (Expression.Type != ExpressionType.Function)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, "Only functions are allowed"));
            return false;
        }
        return valid;
    }
}