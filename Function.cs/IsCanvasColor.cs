
public class IsCanvasColor : Expression
{
    public override ExpressionType Type { get; set; } = ExpressionType.Function;
    public override object? Value { get; set; }
    Expression color;
    Expression horizontal;
    Expression vertical;
    Canvas Canvas;
    public IsCanvasColor(CodeLocation location, Expression color, Expression horizontal, Expression vertical, Canvas canvas) : base(location)
    {
        this.color = color;
        this.horizontal = horizontal;
        this.vertical = vertical;
        Canvas = canvas;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        bool isValid = true;
        if (!horizontal.checksemantic(context, errors) || horizontal.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(horizontal.location, ErrorCode.Invalid, "Horizontal requires to be a number"));
            isValid = false;
        }
        if (!vertical.checksemantic(context, errors) || vertical.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(vertical.location, ErrorCode.Invalid, "Vertical requires to be a number"));
            isValid = false;
        }
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
        Canvas.IsCanvasColor((string)color.Value, (int)vertical.Value, (int)horizontal.Value);
    }
}