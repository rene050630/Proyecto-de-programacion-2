
using System.Linq.Expressions;

public class Color : Statement
{
    Expression color;
    public Color(CodeLocation location, Expression color) : base(location)
    {
        this.color = color;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        bool isValid = true;
        if (!color.checksemantic(context, errors) || color.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, "Colors requieres to be strings"));
            isValid = false;
        }
        if (color.Value is string stringLiteral &&
            !context.IsValidColor(stringLiteral))
        {
            errors.Add(new CompilingError(
                location,
                ErrorCode.Invalid,
                $"Color '{stringLiteral}' is invalid. Allowed colors: {string.Join(", ", context.ValidColors)}"
            ));
            isValid = false;
        }
        return isValid;
    }
    public override void Execute(ExecutionContext context)
    {
        // Actualizar el color del pincel
        context.BrushColor = (string)color.Value;
    }
}