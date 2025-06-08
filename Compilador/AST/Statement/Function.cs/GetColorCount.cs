
public class GetColorCount : Statement
{
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
        x1.Evaluate();
        x2.Evaluate();
        y1.Evaluate();
        y2.Evaluate();
        color.Evaluate();
        int X1 = Convert.ToInt32(x1.Value);
        int X2 = Convert.ToInt32(x2.Value);
        int Y1 = Convert.ToInt32(y1.Value);
        int Y2 = Convert.ToInt32(y2.Value);
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
        if (X1 < 0)
        {
            errors.Add(new CompilingError(x1.location, ErrorCode.Invalid, "x1 requires to be >= 0"));
            isValid = false;
        }
        if (Y1 < 0)
        {
            errors.Add(new CompilingError(x1.location, ErrorCode.Invalid, "y1 requires to be >= 0"));
            isValid = false;
        }
        if (X2 < 0)
        {
            errors.Add(new CompilingError(x1.location, ErrorCode.Invalid, "x2 requires to be >= 0"));
            isValid = false;
        }
        if (Y2 < 0)
        {
            errors.Add(new CompilingError(x1.location, ErrorCode.Invalid, "y2 requires to be >= 0"));
            isValid = false;
        }
        return isValid;
    }
    public override void Execute()
    {
        x1.Evaluate();
        x2.Evaluate();
        y1.Evaluate();
        y2.Evaluate();
        color.Evaluate();
        int X1 = Convert.ToInt32(x1.Value);
        int X2 = Convert.ToInt32(x2.Value);
        int Y1 = Convert.ToInt32(y1.Value);
        int Y2 = Convert.ToInt32(y2.Value);
        string colorValue = (string)color.Value;
        Canvas.GetColorCount(X1, X2, Y1, Y2, colorValue);
    }
}