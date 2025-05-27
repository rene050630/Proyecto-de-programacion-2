public class Variable : Expression
{
    public string variable;
    public override object? Value { get; set; }
    public override ExpressionType Type { get; set; }
    private Scope scope { get; set; }
    public Variable(string variable, CodeLocation location) : base(location)
    {
        this.variable = variable;
    }
    public override void Evaluate(ExecutionContext context)
    {
        Value = scope.GetValue(variable);
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        this.scope = scope;
        if (scope.GetType(variable) == ExpressionType.ErrorType)
        {
            Type = ExpressionType.ErrorType;
            errors.Add(new CompilingError(location, ErrorCode.Invalid, "variable undefined"));
            return false;
        }
        else
        {
            scope.GetType(variable);
            return true;
        }
    }
}