public class GoTo : Statement
{
    public string Label { get; }
    public Expression Condition { get; }
    private Context Context;
    public GoTo(CodeLocation location, string label, Expression condition, Context context) : base(location)
    {
        Label = label;
        Condition = condition;
        Context = context;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        if (!context.LabelExists(Label))
        {
            errors.Add(new CompilingError(location, ErrorCode.UndefinedLabel, $"Label '{Label}' undefined"));
            return false;
        }
        if (!Condition.checksemantic(context, errors) ||
            Condition.Type != ExpressionType.Boolean)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, "Condition requires to be booelan"));
            return false;
        }
        return true;
    }
    public override void Execute()
    {
        // Evaluar la condición
        Condition.Evaluate();

        if (Condition.Value is bool shouldJump && shouldJump)
        {
            // Saltar a la posición de la etiqueta
            Context.JumpToLabel(Label);
        }

    }
}