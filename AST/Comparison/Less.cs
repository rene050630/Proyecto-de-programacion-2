public class Less : BinaryExpression
{
    public override ExpressionType Type { get; set; }
    public override object? Value { get; set; }
    public Less(CodeLocation location) : base(location) { }
    public override void Evaluate(ExecutionContext context)
    {
        Right.Evaluate(context);
        Left.Evaluate(context);

        Value = (double)Left.Value < (double)Right.Value;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        bool right = Right.checksemantic(context, errors);
        bool left = Left.checksemantic(context, errors);
        if ((Right.Type != ExpressionType.Number) && (Left.Type != ExpressionType.Number))
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, "Both operands must be numbers"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        Type = ExpressionType.Number;
        return left && right;
    }
    public override string ToString()
    {
        if (Value == null)
        {
            return string.Format("({0} < {1})", Left, Right);
        }
        return Value.ToString();
    }
}