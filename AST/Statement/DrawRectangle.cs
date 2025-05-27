
public class DrawRectangle : Statement
{
    Expression dirX;
    Expression dirY;
    Expression width;
    Expression height;
    Expression distance;

    public DrawRectangle(CodeLocation location, Expression dirX, Expression dirY, Expression width, Expression height, Expression distance) : base(location)
    {
        this.dirX = dirX;
        this.dirY = dirY;
        this.width = width;
        this.height = height;
        this.distance = distance;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        int centerX = context.Canvas.Walle.ActualX + (int)dirX.Value * (int)distance.Value;
        int centerY = context.Canvas.Walle.ActualY + (int)dirY.Value * (int)distance.Value;
        context.Canvas.IsWithinCanvas(centerX, centerY, context.Canvas.Size);
        bool isValid = true;
        if (!context.Canvas.IsWithinCanvas(centerX, centerY, context.Canvas.Size))
            errors.Add(new CompilingError(location, ErrorCode.Invalid, "Center is outside the limits of the canvas"));
        if (!dirX.checksemantic(context, errors) || dirX.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(
                dirX.location,
                ErrorCode.Invalid,
                "Direction X requires to be a number"
            ));
            isValid = false;
        }

        if (!dirY.checksemantic(context, errors) || dirY.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(
                dirY.location,
                ErrorCode.Invalid,
                "Direction Y requires to be a number"
            ));
            isValid = false;
        }
        if (!distance.checksemantic(context, errors) || distance.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(
                distance.location,
                ErrorCode.Invalid,
                "distance requires to be a number"
            ));
            isValid = false;
        }
        if (!width.checksemantic(context, errors) || width.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(
                width.location,
                ErrorCode.Invalid,
                "Width requires to be a number"
            ));
            isValid = false;
        }
        if (!height.checksemantic(context, errors) || height.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(
                height.location,
                ErrorCode.Invalid,
                "Height requires to be a number"
            ));
            isValid = false;
        }
        if ((int)dirX.Value < -1 || (int)dirX.Value > 1)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, $"Direction X invalid: {(int)dirX.Value}. Allowed values: -1, 0, 1"));
            isValid = false;
        }
        if ((int)dirY.Value < -1 || (int)dirY.Value > 1)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, $"Direction Y invalid: {(int)dirY.Value}. Allowed values: -1, 0, 1"));
            isValid = false;
        }
        if ((int)distance.Value < 1)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, $"Invalid distance: {(int)distance.Value}. It requires to be ≥ 1"));
            isValid = false;
        }
        if ((int)width.Value < 1)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, $"Invalid width: {(int)width.Value}. It requires to be ≥ 1"));
            isValid = false;
        }
        if ((int)height.Value < 1)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, $"Invalid height: {(int)height.Value}. It requires to be ≥ 1"));
            isValid = false;
        }
        return isValid;
    }
    public override void Execute(ExecutionContext context)
    {
        int centerX = context.Canvas.Walle.ActualX + (int)dirX.Value * (int)distance.Value;
        int centerY = context.Canvas.Walle.ActualY + (int)dirY.Value * (int)distance.Value;
        // Dibujar rectángulo
        context.Canvas.DrawRectangleOutline(centerX, centerY, (int)width.Value, (int)height.Value);
        // Actualizar posición de Wall-E
        context.Canvas.Walle.MoveTo(centerX, centerY);
    }
}