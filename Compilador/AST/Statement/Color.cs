
using System.Linq.Expressions;

public class Color : Statement
{
    Expression color;
    Canvas Canvas;
    public Color(CodeLocation location, Expression color, Canvas canvas) : base(location)
    {
        this.color = color;
        Canvas = canvas; 
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        color.Evaluate();
        bool isValid = true;
        if (!color.checksemantic(context, errors) || color.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(color.location, ErrorCode.Invalid, "Colors requieres to be strings"));
            isValid = false;
        }
        if (color.Value is string stringLiteral &&
            !context.IsValidColor(stringLiteral))
        {
            errors.Add(new CompilingError(
                color.location,
                ErrorCode.Invalid,
                $"Color '{stringLiteral}' is invalid. Allowed colors: {string.Join(", ", context.ValidColors)}"
            ));
            isValid = false;
        }
        return isValid;
    }
    public override void Execute()
    {
        color.Evaluate();
        // Actualizar el color del pincel
        Canvas.BrushColor = (string)color.Value;
    }
}