
 //Posible implementacion de una interfaz IExecution con un metodo void Execute para las funciones
public class GoTo : AST
{
    public string Label { get; }
    public Expression Condition { get; }
    ExecutionContext Context;
    public GoTo(CodeLocation location, string label, Expression condition) : base(location)
    {
        Label = label;
        Condition = condition;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        if (!context.LabelExists(Label))
        {
            errors.Add(new CompilingError(location,
                ErrorCode.UndefinedLabel,
                $"Label '{Label}' undefined"));
            return false;
        }
        if (!Condition.checksemantic(context, errors) ||
            Condition.Type != ExpressionType.Boolean)
        {
            errors.Add(new CompilingError(location,
                ErrorCode.Invalid,
                "Condition requires to be booelan"));
            return false;
        }
        return true;
    }
    public void Execution(Context context)
    {
        // Evaluar la condición
        Condition.Evaluate(Context);

        if (Condition.Value is bool shouldJump && shouldJump)
        {
            // Saltar a la posición de la etiqueta
            context.JumpToLabel(Label);
        }

    }
}