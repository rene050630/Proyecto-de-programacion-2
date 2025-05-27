public class Number : AtomsExpression
{
    public bool IsInt
    {
        get
        {
            int a;
            return int.TryParse(Value.ToString(), out a);
        }
    }

    public override ExpressionType Type
    {
        get
        {
            return ExpressionType.Number;
        }
        set { }
    }

    public override object? Value { get; set; }
    
    public Number(double value, CodeLocation location) : base(location)
    {
        Value = value;
    }
    
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        return true;
    }

    public override void Evaluate(ExecutionContext context)
    {
        
    }

    public override string ToString()
    {
        return string.Format("{0}",Value);
    }
}