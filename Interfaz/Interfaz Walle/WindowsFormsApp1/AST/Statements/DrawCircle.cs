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
            if (!dirX.checksemantic(context, errors) || dirX.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(
                    dirX.location,
                    ErrorCode.Invalid,
                    "Direction X requires to be a number"
                ));
                return false;
            }

            if (!dirY.checksemantic(context, errors) || dirY.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(
                    dirY.location,
                    ErrorCode.Invalid,
                    "Direction Y requires to be a number"
                ));
                return false;
            }
            if (!radius.checksemantic(context, errors) || radius.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(
                    radius.location,
                    ErrorCode.Invalid,
                    "Radius requires to be a number"
                ));
                return false;
            }
            dirX.Evaluate();
            dirY.Evaluate();
            radius.Evaluate();
            int dirXInt = Convert.ToInt32(dirX.Value);
            int dirYInt = Convert.ToInt32(dirY.Value);
            int radiusInt = Convert.ToInt32(radius.Value);
            int centerX = Canvas.ActualX + dirXInt * radiusInt;
            int centerY = Canvas.ActualY + dirYInt * radiusInt;
            if (dirXInt < -1 || dirXInt > 1)
            {
                errors.Add(new CompilingError(dirX.location, ErrorCode.Invalid, $"Direction X invalid: {dirXInt}. Allowed values: -1, 0, 1"));
                return false;
            }
            if (dirYInt < -1 || dirYInt > 1)
            {
                errors.Add(new CompilingError(dirY.location, ErrorCode.Invalid, $"Direction Y invalid: {dirYInt}. Allowed values: -1, 0, 1"));
                return false;
            }
            if (radiusInt < 1)
            {
                errors.Add(new CompilingError(radius.location, ErrorCode.Invalid, $"Invalid radius: {radiusInt}. It requires to be ≥ 1"));
                return false;
            }
            if (!Canvas.IsWithinCanvas(centerX, centerY, Canvas.Size))
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid, "Center is outside the limits of the canvas"));
                return false;
            }
            return true;
        }
        public override void Execute()
        {
            dirX.Evaluate();
            dirY.Evaluate();
            radius.Evaluate();
            int centerX = Canvas.ActualX + Convert.ToInt32(dirX.Value) * Convert.ToInt32(radius.Value);
            int centerY = Canvas.ActualY + Convert.ToInt32(dirY.Value) * Convert.ToInt32(radius.Value);
            Canvas.DrawMidpointCircle(centerX,centerY, Convert.ToInt32(radius.Value));
            Canvas.ActualX = centerX;
            Canvas.ActualY = centerY;
        }
    }
}