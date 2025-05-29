
public class GetColorCount : Expression
{
    public override ExpressionType Type { get; set; } = ExpressionType.Function;
    public override object? Value { get; set; }
    Canvas Canvas;
    Expression color;
    Expression x1;
    Expression y1;
    Expression x2;
    Expression y2;
    public GetColorCount(CodeLocation location, Canvas canvas, Expression color, Expression x1, Expression y1,
    Expression x2, Expression y2) : base(location)
    {
        Canvas = canvas;
        this.color = color;
        this.x1 = x1;
        this.x2 = x2;
        this.y1 = y1;
        this.y2 = y2;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        bool isValid = true;
        if (!x1.checksemantic(context, errors) || x1.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(x1.location, ErrorCode.Invalid, "x1 requires to be a number"));
            isValid = false;
        }
        if (!x2.checksemantic(context, errors) || x2.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(x2.location, ErrorCode.Invalid, "x2 requires to be a number"));
            isValid = false;
        }
        if (!y1.checksemantic(context, errors) || y1.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(y1.location, ErrorCode.Invalid, "y1 requires to be a number"));
            isValid = false;
        }
        if (!y2.checksemantic(context, errors) || y2.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(y2.location, ErrorCode.Invalid, "y2 requires to be a number"));
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
        if ((int)x1.Value < 0)
        {
            errors.Add(new CompilingError(x1.location, ErrorCode.Invalid, "x1 requires to be >= 0"));
            isValid = false;
        }
        if ((int)y1.Value < 0)
        {
            errors.Add(new CompilingError(x1.location, ErrorCode.Invalid, "y1 requires to be >= 0"));
            isValid = false;
        }
        if ((int)x2.Value < 0)
        {
            errors.Add(new CompilingError(x1.location, ErrorCode.Invalid, "x2 requires to be >= 0"));
            isValid = false;
        }
        if ((int)y2.Value < 0)
        {
            errors.Add(new CompilingError(x1.location, ErrorCode.Invalid, "y2 requires to be >= 0"));
            isValid = false;
        }
        return isValid;
    }
    public override void Evaluate()
    {
        Canvas.GetColorCount((int)x1.Value, (int)x2.Value, (int)y1.Value, (int)y2.Value, (string)color.Value);
    }
}