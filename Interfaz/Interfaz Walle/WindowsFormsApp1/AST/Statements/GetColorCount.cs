
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Remoting.Contexts;
namespace WindowsFormsApp1
{
    public class GetColorCount : Expression
    {
        Canvas Canvas;
        Expression color;
        Expression x1;
        Expression y1;
        Expression x2;
        Expression y2;
        List<CompilingError> errors;
        public override object Value { get; set; }
        public override ExpressionType Type
        {
            get { return ExpressionType.Number; }
            set { }
        }
        public GetColorCount(CodeLocation location, Canvas canvas, Expression color, Expression x1, Expression y1,
        Expression x2, Expression y2, List<CompilingError> errors) : base(location)
        {
            Canvas = canvas;
            this.color = color;
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;
            this.errors = errors;
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            if (!x1.checksemantic(context, errors) || x1.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(x1.location, ErrorCode.Invalid, "x1 requires to be a number"));
                return false;
            }
            if (!x2.checksemantic(context, errors) || x2.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(x2.location, ErrorCode.Invalid, "x2 requires to be a number"));
                return false;
            }
            if (!y1.checksemantic(context, errors) || y1.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(y1.location, ErrorCode.Invalid, "y1 requires to be a number"));
                return false;
            }
            if (!y2.checksemantic(context, errors) || y2.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(y2.location, ErrorCode.Invalid, "y2 requires to be a number"));
                return false;
            }
            if (!color.checksemantic(context, errors) || color.Type != ExpressionType.Text)
            {
                errors.Add(new CompilingError(color.location, ErrorCode.Invalid, "color requires to be a string"));
                return false;
            }
            color.Evaluate();
            if (color.Value == null)
            {
                errors.Add(new CompilingError(color.location, ErrorCode.Invalid, "Colors cannot be null"));
                return false;
            }
            if (color.Value is string stringLiteral &&
                !context.IsValidColor(stringLiteral.ToLower()))
            {
                errors.Add(new CompilingError(color.location, ErrorCode.Invalid,
                    $"Color '{stringLiteral}' is invalid. Allowed colors: {string.Join(", ", context.ValidColors)}"
                ));
            }
            return true;
        }
        public override void Evaluate()
        {
            x1.Evaluate();
            x2.Evaluate();
            y1.Evaluate();
            y2.Evaluate();
            color.Evaluate();
            int X1 = Convert.ToInt32(x1.Value);
            int X2 = Convert.ToInt32(x2.Value);
            int Y1 = Convert.ToInt32(y1.Value);
            int Y2 = Convert.ToInt32(y2.Value);
            if (X1 < 0)
            {
                errors.Add(new CompilingError(x1.location, ErrorCode.Invalid, "x1 requires to be >= 0"));
                return;
            }
            if (Y1 < 0)
            {
                errors.Add(new CompilingError(x1.location, ErrorCode.Invalid, "y1 requires to be >= 0"));
                return;
            }
            if (X2 < 0)
            {
                errors.Add(new CompilingError(x1.location, ErrorCode.Invalid, "x2 requires to be >= 0"));
                return;
            }
            if (Y2 < 0)
            {
                errors.Add(new CompilingError(x1.location, ErrorCode.Invalid, "y2 requires to be >= 0"));
                return;
            }
            Colors colorValue = Color();
            Value = Canvas.GetColorCount(X1, X2, Y1, Y2, colorValue);
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