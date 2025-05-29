
public class GreaterT : BinaryExpression
{
    public override ExpressionType Type { get; set; }
    public override object? Value { get; set; }
    public GreaterT(CodeLocation location, Expression left, Expression right) : base(location)
    {
        Left = left;
        Right = right;
    }
    public override void Evaluate()
    {
        Right.Evaluate();
        Left.Evaluate();

        Value = (double)Left.Value >= (double)Right.Value;
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
            return string.Format("({0} >= {1})", Left, Right);
        }
        return Value.ToString();
    }
}