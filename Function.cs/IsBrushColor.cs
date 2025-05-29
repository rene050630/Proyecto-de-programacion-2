
public class IsBrushColor : Expression
{
    public override ExpressionType Type { get; set; } = ExpressionType.Function;
    public override object? Value { get; set; }
    Expression color;
    Canvas Canvas;
    public IsBrushColor(CodeLocation location, Expression color, Canvas canvas) : base(location)
    {
        Canvas = canvas;
        this.color = color;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        bool isValid = false;
        if (!color.checksemantic(context, errors) || color.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(color.location, ErrorCode.Invalid, "color requires to be a string"));
            isValid = false;
        }
        if (color.Value is string stringLiteral &&
            !context.IsValidColor(stringLiteral))
        {
            errors.Add(new CompilingError(color.location, ErrorCode.Invalid,
                $"Color '{stringLiteral}' is invalid. Allowed colors: {string.Join(", ", context.ValidColors)}"
            ));
            isValid = false;
        }
        return isValid;
    }
    public override void Evaluate()
    {
        Canvas.IsBrushColor((string)color.Value);
    }
}