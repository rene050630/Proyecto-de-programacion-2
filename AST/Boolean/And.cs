public class And : BinaryExpression
{
    public override ExpressionType Type{get;set;}
    public override object? Value{get;set;}
    public And(CodeLocation location) : base(location) {}
    public override void Evaluate(ExecutionContext context)
    {
        Right.Evaluate(context);
        Left.Evaluate(context);
        
        Value = (bool)Left.Value && (bool)Right.Value;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        bool right = Right.checksemantic(context, errors);
        bool left = Left.checksemantic(context, errors);
        if ((Right.Type != ExpressionType.Boolean) || (Left.Type != ExpressionType.Boolean))
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, "Operator '&&' requires boolean operands"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        Type = ExpressionType.Boolean;
        return right && left;
    }
}