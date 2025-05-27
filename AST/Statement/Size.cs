
public class Size : Statement
{
    Expression size;
    public Size(CodeLocation location, Expression size) : base(location)
    {
        this.size = size;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        bool isValid = true;
        if (!size.checksemantic(context, errors) || size.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, "Size requieres to be a number"));
            isValid = false;
        }
        if (size.Value is int numericLiteral)
        {
            var value = Convert.ToDouble(numericLiteral);

            if (value < 1)
            {
                errors.Add(new CompilingError(
                    location,
                    ErrorCode.Invalid,
                    "El tamaño mínimo del pincel es 1"
                ));
                isValid = false;
            }
        }
        return isValid;
    }
    public override void Execute(ExecutionContext context)
    {
        int size = (int)Math.Floor((double)this.size.Value);
        // Ajustar a tamaño válido
        size = Math.Max(1, size);       // Mínimo 1
        size = size % 2 == 0 ? size - 1 : size; // Convertir a impar
        context.BrushSize = size;
    }
}