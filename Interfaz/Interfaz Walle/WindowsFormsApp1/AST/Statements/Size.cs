
using System.Collections.Generic;
using System;

namespace WindowsFormsApp1
{
    public class Sizes : Statement
    {
        Expression size;
        Canvas Canvas;
        List <CompilingError> errors;
        public Sizes(CodeLocation location, Expression size, Canvas canvas, List<CompilingError> errors) : base(location)
        {
            this.size = size;
            Canvas = canvas;
            this.errors = errors;
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            if (!size.checksemantic(context, errors) || size.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid, "Size requieres to be a number"));
                return false;
            }
   
            return true;
        }
        public override void Execute()
        {
            size.Evaluate();
            int sizeInt = Convert.ToInt32(size.Value);
            size.Evaluate();
            if (sizeInt < 1)
            {
                 errors.Add(new CompilingError(
                 location,
                 ErrorCode.Invalid,
                 "Minimum brush size: 1"
                ));
                return;
            }
            sizeInt = Math.Max(1, sizeInt);
            sizeInt = sizeInt % 2 == 0 ? sizeInt - 1 : sizeInt;
            Canvas.BrushSize = sizeInt;
        }
    }
}