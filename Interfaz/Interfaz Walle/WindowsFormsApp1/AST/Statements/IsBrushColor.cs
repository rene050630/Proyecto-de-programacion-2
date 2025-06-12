
using System.Collections.Generic;

namespace WindowsFormsApp1
{  
    public class IsBrushColor : Expression
    {
        Expression color;
        Canvas Canvas;
        public override object Value { get; set; }
        public override ExpressionType Type
        {
            get { return ExpressionType.Number; }
            set { }
        }
        public IsBrushColor(CodeLocation location, Expression color, Canvas canvas) : base(location)
        {
            Canvas = canvas;
            this.color = color;
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            if (!color.checksemantic(context, errors) || color.Type != ExpressionType.Text)
            {
                errors.Add(new CompilingError(color.location, ErrorCode.Invalid, "color requires to be a string"));
                return false;
            }
            string colorValue = color.Value.ToString().ToLower();
            if (!context.IsValidColor(colorValue))
            {
                errors.Add(new CompilingError(
                    color.location,
                    ErrorCode.Invalid,
                    $"Color '{colorValue}' is invalid. Allowed colors: {string.Join(", ", context.ValidColors)}"
                ));
                return false;
            }
            return true;
        }
        public override void Evaluate()
        {
            color.Evaluate();
            string colorValue = (string)color.Value;
            Value = Canvas.IsBrushColor(GetColor(colorValue));
        }
        public Colors GetColor(string colorValue)
        {
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