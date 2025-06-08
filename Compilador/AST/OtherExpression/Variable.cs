//Error en GetType
public class Variable : Expression
{
    public string variable;
    public override ExpressionType Type { get; set; }
    public override object? Value { get; set; }
    private Context context;
    public Variable(CodeLocation location, string variable, Context context) : base(location)
    {
        this.variable = variable;
        this.context = context;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        if (context.GetValue(variable) == null)
        {
            context.SetType(variable, ExpressionType.Text);
        }
        else
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, "Variable is already defined"));
            return false;
        }
        if (context.GetType(variable) == ExpressionType.ErrorType)
        {
            errors.Add(new CompilingError(location, ErrorCode.UndefinedLabel, "Variable is undefined"));
            return false;
        }
        else return true;
    }
    public override void Evaluate()
    {
        Value = context.Execute(variable);
    }
}