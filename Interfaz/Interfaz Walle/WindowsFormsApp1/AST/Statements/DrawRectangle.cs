
using System.Collections.Generic;
using System;

namespace WindowsFormsApp1
{
    public class DrawRectangle : Statement
    {
        Expression dirX;
        Expression dirY;
        Expression width;
        Expression height;
        Expression distance;
        Canvas Canvas;

        public DrawRectangle(CodeLocation location, Expression dirX, Expression dirY, Expression distance, Expression width, Expression height, Canvas canvas) : base(location)
        {
            this.dirX = dirX;
            this.dirY = dirY;
            this.width = width;
            this.height = height;
            this.distance = distance;
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
            if (!distance.checksemantic(context, errors) || distance.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(
                    distance.location,
                    ErrorCode.Invalid,
                    "distance requires to be a number"
                ));
                return false;
            }
            if (!width.checksemantic(context, errors) || width.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(
                    width.location,
                    ErrorCode.Invalid,
                    "Width requires to be a number"
                ));
                return false;
            }
            if (!height.checksemantic(context, errors) || height.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(
                    height.location,
                    ErrorCode.Invalid,
                    "Height requires to be a number"
                ));
                return false;
            }
            dirX.Evaluate();
            dirY.Evaluate();
            distance.Evaluate();
            width.Evaluate();
            height.Evaluate();
            int dirXInt = Convert.ToInt32(dirX.Value);
            int dirYInt = Convert.ToInt32(dirY.Value);
            int distanceInt = Convert.ToInt32(distance.Value);
            int widthInt = Convert.ToInt32(width.Value);
            int heightInt = Convert.ToInt32(height.Value);
            int centerX = Canvas.ActualX + dirXInt * distanceInt;
            int centerY = Canvas.ActualY + dirYInt * distanceInt;
            if (!Canvas.IsWithinCanvas(centerX, centerY, Canvas.Size))
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid, "Center is outside the limits of the canvas"));
                return false;
            }
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
            if (distanceInt < 1)
            {
                errors.Add(new CompilingError(distance.location, ErrorCode.Invalid, $"Invalid distance: {distanceInt}. It requires to be ≥ 1"));
                return false;
            }
            if (widthInt < 1)
            {
                errors.Add(new CompilingError(width.location, ErrorCode.Invalid, $"Invalid width: {widthInt}. It requires to be ≥ 1"));
                return false;
            }   
            if (heightInt < 1)
            {
                errors.Add(new CompilingError(height.location, ErrorCode.Invalid, $"Invalid height: {heightInt}. It requires to be ≥ 1"));
                return false;
            }
            return true;
        }
        public override void Execute()
        {
            dirX.Evaluate();
            dirY.Evaluate();
            distance.Evaluate();
            width.Evaluate();
            height.Evaluate();
            int dirXInt = Convert.ToInt32(dirX.Value);
            int dirYInt = Convert.ToInt32(dirY.Value);
            int distanceInt = Convert.ToInt32(distance.Value);
            int widthInt = Convert.ToInt32(width.Value);
            int heightInt = Convert.ToInt32(height.Value);
            int centerX = Canvas.ActualX + dirXInt * distanceInt;
            int centerY = Canvas.ActualY + dirYInt * distanceInt;
            // Dibujar rectángulo
            Canvas.DrawRectangleOutline(centerX, centerY, widthInt, heightInt);
            // Actualizar posición de Wall-E
            Canvas.MoveTo(centerX, centerY);
        }
    }
}