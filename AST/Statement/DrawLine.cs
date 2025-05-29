using System.Dynamic;

public enum Direction{Right, Left, Up, Down}
public class DrawLine : Statement
{
    Expression distance;
    Expression dirX;
    Expression dirY;
    Canvas Canvas;
    public DrawLine(CodeLocation location, Expression distance, Expression dirX, Expression dirY, Canvas canvas) : base(location)
    {
        this.distance = distance;
        this.dirX = dirX;
        this.dirY = dirY;
        Canvas = canvas;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        bool isValid = true;
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
                "Distance requires to be a number"
            ));
            isValid = false;
        }
        if ((int)dirX.Value < -1 || (int)dirX.Value > 1)
        {
            errors.Add(new CompilingError(dirX.location, ErrorCode.Invalid, $"Direction X invalid: {(int)dirX.Value}. Allowed values: -1, 0, 1"));
            isValid = false;
        }
        if ((int)dirY.Value < -1 || (int)dirY.Value > 1)
        {
            errors.Add(new CompilingError(dirY.location, ErrorCode.Invalid, $"Direction Y invalid: {(int)dirY.Value}. Allowed values: -1, 0, 1"));
            isValid = false;
        }
        if ((int)distance.Value < 1)
        {
            errors.Add(new CompilingError(distance.location, ErrorCode.Invalid, $"Invalid distance: {(int)distance.Value}. It requires to be ≥ 1"));
            isValid = false;
        }
        return isValid;
    }
    public override void Execute()
    {
        int intDirX = (int)dirX.Value;
        int intDirY = (int)dirY.Value;
        int intDistance = (int)distance.Value;
        // Ejecutar en el canvas
        Canvas.DrawLine(intDirX, intDirY, intDistance);
    }
}