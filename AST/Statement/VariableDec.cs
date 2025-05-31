public class VariableDec : Statement
{
    public string Name { get; }
    public Expression Expression { get; }

    public VariableDec(string Name, Expression expression, CodeLocation location)
        : base(location)
    {
        this.Name = Name;
        Expression = expression;
    }
    public override void Execute()
    {
        // Evaluar la expresión
        Expression.Evaluate();
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        // 1. Validar nombre de variable
        if (!IsIdentifier(Name))
        {
            errors.Add(new CompilingError(
                location,
                ErrorCode.Invalid,
                $"Variable name is invalid"
            ));
            return false;
        }
        // 2. Validar expresión
        bool ValidExpression = Expression.checksemantic(context, errors);
        if (ValidExpression)
        {
            context.SetValue(Expression.ToString(), Expression.Value);
        }
        return ValidExpression;
    }

    private bool IsIdentifier(string nombre)
    {
        // Implementar reglas del lenguaje:
        // - No puede comenzar con número o _
        // - Solo letras, números y _
        if (string.IsNullOrEmpty(nombre)) return false;
        if (char.IsDigit(nombre[0])) return false;
        if (nombre[0] == '_') return false;
        
        foreach (char c in nombre)
        {
            if (!char.IsLetterOrDigit(c) && c != '_')
                return false;
        }
        
        return true;
    }

    public override string ToString()
    {
        return $"{Name} <- {Expression}";
    }
}