
using System.Collections.Generic;
using System;
namespace WindowsFormsApp1
{
    public class IsCanvasColor : Expression
    {
        Expression color;
        Expression horizontal;
        Expression vertical;
        Canvas Canvas;
        List <CompilingError> errors;
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }
        public IsCanvasColor(CodeLocation location, Expression color, Expression vertical, Expression horizontal, Canvas canvas, List<CompilingError> errors) : base(location)
        {
            this.color = color;
            this.horizontal = horizontal;
            this.vertical = vertical;
            Canvas = canvas;
            this.errors = errors;
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
            if (color.Value is string stringLiteral &&
                !context.IsValidColor(stringLiteral))
            {
                errors.Add(new CompilingError(color.location, ErrorCode.Invalid,
                    $"Color '{stringLiteral}' is invalid. Allowed colors: {string.Join(", ", context.ValidColors)}"
                ));
                return false;
            }
            color.Evaluate();
            vertical.Evaluate();
            horizontal.Evaluate();
            int horizontalInt = Convert.ToInt32(horizontal.Value);
            int verticalInt = Convert.ToInt32(vertical.Value);
            if (horizontalInt + Canvas.ActualX < 0 || horizontalInt + Canvas.ActualX >= Canvas.Size || verticalInt + Canvas.ActualY < 0 || verticalInt + Canvas.ActualY >= Canvas.Size)
            {
                Type = ExpressionType.Boolean;
                return true;
            }
            Type = ExpressionType.Number;
            return true;
        }
        public override void Evaluate()
        {
            color.Evaluate();
            vertical.Evaluate();
            horizontal.Evaluate();
            int horizontalInt = Convert.ToInt32(horizontal.Value);
            int verticalInt = Convert.ToInt32(vertical.Value);
            Colors colorValue = Color();
            if (Canvas.IsPositionValid(Canvas.ActualX + horizontalInt, Canvas.ActualY + verticalInt))
                Value = Canvas.IsCanvasColor(colorValue, verticalInt, horizontalInt);
            else
            {
                Value = Canvas.IsCanvasColor();
            }
        }
        private Colors Color()
        {
            string colorValue = (string)color.Value;
            switch (colorValue.ToLower())
            {
                case "red": return Colors.Red;
                case "blue": return Colors.Blue;
                case "green": return Colors.Green;
                case "yellow": return Colors.Yellow;
                case "black": return Colors.Black;
                case "white": return Colors.White;
                case "orange": return Colors.Orange;
                case "purple": return Colors.Purple;
                case "transparent": return Colors.Transparent;
                default: return Colors.White;
            }
        }
    }
}