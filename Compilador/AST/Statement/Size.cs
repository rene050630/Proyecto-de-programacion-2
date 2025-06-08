
public class Size : Statement
{
    Expression size;
    Canvas Canvas;
    public Size(CodeLocation location, Expression size, Canvas canvas) : base(location)
    {
        this.size = size;
        Canvas = canvas;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        size.Evaluate();
        bool isValid = true;
        if (!size.checksemantic(context, errors) || size.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, "Size requieres to be a number"));
            isValid = false;
        }
        if (size.Value is int numericLiteral)
        {
            var value = Convert.ToInt32(numericLiteral);

            if (value < 1)
            {
                errors.Add(new CompilingError(
                    location,
                    ErrorCode.Invalid,
                    "Minimum brush size: 1"
                ));
                isValid = false;
            }
        }
        return isValid;
    }
    public override void Execute()
    {
        size.Evaluate();
        int sizeInt = Convert.ToInt32(size.Value);
        // Ajustar a tamaño válido
        sizeInt = Math.Max(1, sizeInt);       // Mínimo 1
        sizeInt = sizeInt % 2 == 0 ? sizeInt - 1 : sizeInt; // Convertir a impar
        Canvas.BrushSize = sizeInt;
    }
}