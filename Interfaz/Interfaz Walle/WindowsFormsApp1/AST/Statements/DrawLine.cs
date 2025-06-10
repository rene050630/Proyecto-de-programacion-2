using System.Collections.Generic;
using System;
using System.Dynamic;

namespace WindowsFormsApp1
{
    public enum Direction { Right, Left, Up, Down }
    public class DrawLine : Statement
    {
        Expression distance;
        Expression dirX;
        Expression dirY;
        Canvas Canvas;
        public DrawLine(CodeLocation location, Expression dirX, Expression dirY, Expression distance, Canvas canvas) : base(location)
        {
            this.distance = distance;
            this.dirX = dirX;
            this.dirY = dirY;
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
                    "Distance requires to be a number"
                ));
                return false;
            }
            //dirX.Evaluate();
            //dirY.Evaluate();
            //distance.Evaluate();
            //int distanceInt = Convert.ToInt32(distance.Value);
            //int dirXInt = Convert.ToInt32(dirX.Value);
            //int dirYInt = Convert.ToInt32(dirY.Value);
            //if (dirXInt < -1 || dirXInt > 1)
            //{
            //    errors.Add(new CompilingError(dirX.location, ErrorCode.Invalid, $"Direction X invalid: {dirXInt}. Allowed values: -1, 0, 1"));
            //    return false;
            //}
            //else if (dirYInt < -1 || dirYInt > 1)
            //{
            //    errors.Add(new CompilingError(dirY.location, ErrorCode.Invalid, $"Direction Y invalid: {dirYInt}. Allowed values: -1, 0, 1"));
            //    return false;
            //}
            //else if (distanceInt < 1)
            //{
            //    errors.Add(new CompilingError(distance.location, ErrorCode.Invalid, $"Invalid distance: {distanceInt}. It requires to be ≥ 1"));
            //    return false;
            //}
            return true;
        }
        public override void Execute()
        {
            dirX.Evaluate();
            dirY.Evaluate();
            distance.Evaluate();
            int distanceInt = Convert.ToInt32(distance.Value);
            int dirXInt = Convert.ToInt32(dirX.Value);
            int dirYInt = Convert.ToInt32(dirY.Value);

            // Ejecutar en el canvas
            Canvas.DrawLine(dirXInt, dirYInt, distanceInt);
        }
    }
}