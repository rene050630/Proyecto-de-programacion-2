
using System.Collections.Generic;
using System;
//Arreglar lo de los colores
namespace WindowsFormsApp1
{
    public class IsCanvasColor : Statement
    {
        Expression color;
        Expression horizontal;
        Expression vertical;
        Canvas Canvas;
        public IsCanvasColor(CodeLocation location, Expression color, Expression vertical, Expression horizontal, Canvas canvas) : base(location)
        {
            this.color = color;
            this.horizontal = horizontal;
            this.vertical = vertical;
            Canvas = canvas;
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            if (!horizontal.checksemantic(context, errors) || horizontal.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(horizontal.location, ErrorCode.Invalid, "Horizontal requires to be a number"));
                return false;
            }
            if (!vertical.checksemantic(context, errors) || vertical.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(vertical.location, ErrorCode.Invalid, "Vertical requires to be a number"));
                return false;
            }
            if (!color.checksemantic(context, errors) || color.Type != ExpressionType.Text)
            {
                errors.Add(new CompilingError(color.location, ErrorCode.Invalid, "color requires to be a string"));
                return false;
            }
            color.Evaluate();
            vertical.Evaluate();
            horizontal.Evaluate();
            int X = Convert.ToInt32(vertical.Value);
            int Y = Convert.ToInt32(horizontal.Value);
            if (X + Canvas.ActualX < 0 || X + Canvas.ActualX >= Canvas.Size || Y + Canvas.ActualY < 0 || Y + Canvas.ActualY >= Canvas.Size)
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid, "La casilla tiene que estar dentro de las dimensiones del canvas"));
                return false;
            }
            if (color.Value is string stringLiteral &&
                !context.IsValidColor(stringLiteral))
            {
                errors.Add(new CompilingError(color.location, ErrorCode.Invalid,
                    $"Color '{stringLiteral}' is invalid. Allowed colors: {string.Join(", ", context.ValidColors)}"
                ));
                return false;
            }
            return true;
        }
        public override void Execute()
        {
            color.Evaluate();
            vertical.Evaluate();
            horizontal.Evaluate();
            int horizontalInt = Convert.ToInt32(horizontal.Value);
            int verticalInt = Convert.ToInt32(vertical.Value);
            if (Canvas.IsPositionValid(Canvas.ActualX + horizontalInt, Canvas.ActualY + verticalInt))
                Canvas.IsCanvasColor(Colors.Red, verticalInt, horizontalInt);
            else Canvas.IsCanvasColor();
        }
    }
}