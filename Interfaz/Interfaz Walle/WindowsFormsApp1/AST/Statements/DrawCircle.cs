using System.Collections.Generic;
using System;

namespace WindowsFormsApp1
{
    public class DrawCircle : Statement
    {
        Expression dirX;
        Expression dirY;
        Expression radius;
        Canvas Canvas;
        public DrawCircle(CodeLocation location, Expression dirX, Expression dirY, Expression radius, Canvas canvas) : base(location)
        {
            this.dirX = dirX;
            this.dirY = dirY;
            this.radius = radius;
            Canvas = canvas;
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            dirX.Evaluate();
            dirY.Evaluate();
            radius.Evaluate();
            int dirXInt = Convert.ToInt32(dirX.Value);
            int dirYInt = Convert.ToInt32(dirY.Value);
            int radiusInt = Convert.ToInt32(radius.Value);
            int centerX = Canvas.ActualX + dirXInt * radiusInt;
            int centerY = Canvas.ActualY + dirYInt * radiusInt;
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
            if (!radius.checksemantic(context, errors) || radius.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(
                    radius.location,
                    ErrorCode.Invalid,
                    "Radius requires to be a number"
                ));
                isValid = false;
            }

            if (dirXInt < -1 || dirXInt > 1)
            {
                errors.Add(new CompilingError(dirX.location, ErrorCode.Invalid, $"Direction X invalid: {dirXInt}. Allowed values: -1, 0, 1"));
            }
            if (dirYInt < -1 || dirYInt > 1)
            {
                errors.Add(new CompilingError(dirY.location, ErrorCode.Invalid, $"Direction Y invalid: {dirYInt}. Allowed values: -1, 0, 1"));
            }
            if (radiusInt < 1)
            {
                errors.Add(new CompilingError(radius.location, ErrorCode.Invalid, $"Invalid radius: {radiusInt}. It requires to be ≥ 1"));
            }
            if (!Canvas.IsWithinCanvas(centerX, centerY, Canvas.Size))
                errors.Add(new CompilingError(location, ErrorCode.Invalid, "Center is outside the limits of the canvas"));
            return isValid;
        }
        public override void Execute()
        {
            dirX.Evaluate();
            dirY.Evaluate();
            radius.Evaluate();
        }
    }
}