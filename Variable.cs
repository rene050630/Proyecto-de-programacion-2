
public class Variable : Expression
{
    public string variable;
    public override ExpressionType Type { get; set; }
    public override object? Value { get; set; }
    private Context context;
    public Variable(CodeLocation location, string variable) : base(location)
    {
        this.variable = variable;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        if (context.GetType(variable) == ExpressionType.ErrorType)
        {
            errors.Add(new CompilingError(location, ErrorCode.UndefinedLabel, "Variable is undefined"));
            return false;
        }
        else return true;
    }
    public override void Evaluate()
    {
        Value = context.GetType(variable);
    }
}