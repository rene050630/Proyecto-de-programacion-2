public class Equal : BinaryExpression
{
    public override ExpressionType Type{get;set;}
    public override object? Value{get; set;}
    public Equal(CodeLocation location) : base(location)
    {
    }
    public override void Evaluate()
    {
        Right.Evaluate();
        Left.Evaluate();
        if(Right.Type == ExpressionType.Number && Left.Type == ExpressionType.Number)
        {
            this.Value = (double)this.Left.Value == (double)this.Right.Value;
        }
        else if(Right.Type == ExpressionType.Boolean && Left.Type == ExpressionType.Boolean)
        {
            this.Value = (bool)this.Left.Value == (bool)this.Right.Value;
        }
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        bool right = Right.checksemantic(context, errors);
        bool left = Left.checksemantic(context, errors);
        if(((Right.Type == ExpressionType.Number) && (Left.Type == ExpressionType.Number)) || 
         ((Right.Type == ExpressionType.Boolean) && (Left.Type == ExpressionType.Boolean)))
        {
            Type = ExpressionType.Boolean;
            return right && left;
        }
        else
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, $"Cannot compare {Left.Type} and {Right.Type}"));
            Type = ExpressionType.ErrorType;
            return false;
        }
    }
    public override string ToString()
    {
        if (Value == null)
        {
            return string.Format("({0} == {1})", Left, Right);
        }
        return Value.ToString();
    }
}