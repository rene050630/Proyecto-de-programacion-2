public class DrawCircle : Statement
{
    Expression dirX;
    Expression dirY;
    Expression radius;
    public DrawCircle(CodeLocation location, Expression dirX, Expression dirY, Expression radius) : base(location)
    {
        this.dirX = dirX;
        this.dirY = dirY;
        this.radius = radius;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        int centerX = context.Canvas.WalleX.ActualX + (int)dirX.Value * (int)radius.Value;
        int centerY = context.Canvas.WalleY.ActualY + (int)dirY.Value * (int)radius.Value;
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
        if (!radius.checksemantic(context, errors) || radius.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(
                radius.location,
                ErrorCode.Invalid,
                "Radius requires to be a number"
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
        if ((int)radius.Value < 1)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, $"Invalid radius: {(int)radius.Value}. It requires to be ≥ 1"));
            isValid = false;
        }
        return isValid;
    }
    public override void Execute(ExecutionContext context)
    {
        int centerX = context.Canvas.WalleX.ActualX + (int)dirX.Value * (int)radius.Value;
        int centerY = context.Canvas.WalleY.ActualY + (int)dirY.Value * (int)radius.Value;
        // Dibujar círculo
        context.Canvas.DrawMidpointCircle(centerX, centerY, (int)radius.Value);

        // Actualizar posición de Wall-E
        context.Canvas.Walle.MoveTo(centerX, centerY);
    }
}