public class Add : BinaryExpression
{
    public override ExpressionType Type { get; set; }
    public override object? Value { get; set; }
    public Add(CodeLocation location) : base(location)
    {
    }
    public override void Evaluate()
    {
        Left.Evaluate();
        Right.Evaluate();
        
        Value = (double)Right.Value + (double)Left.Value;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        bool right = Right.checksemantic(context, errors);
        bool left = Left.checksemantic(context, errors);
        if (Right.Type != ExpressionType.Number || Left.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, "We don't do that here... "));
            Type = ExpressionType.ErrorType;
            return false;
        }

        Type = ExpressionType.Number;
        return right && left;
    }
    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("({0} + {1})", Left, Right);
        }
        return Value.ToString();
    }
}