
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WindowsFormsApp1
{
    public class Colores : Statement
    {
        Expression color;
        Canvas Canvas;
        public Colores(CodeLocation location, Expression color, Canvas canvas) : base(location)
        {
            this.color = color;
            Canvas = canvas;
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            color.Evaluate();
            bool isValid = true;
            if (!color.checksemantic(context, errors) || color.Type != ExpressionType.Text)
            {
                errors.Add(new CompilingError(color.location, ErrorCode.Invalid, "Colors requieres to be strings"));
                isValid = false;
            }
            if (color.Value is string stringLiteral &&
                !context.IsValidColor(stringLiteral))
            {
                errors.Add(new CompilingError(
                    color.location,
                    ErrorCode.Invalid,
                    $"Color '{stringLiteral}' is invalid. Allowed colors: {string.Join(", ", context.ValidColors)}"
                ));
                isValid = false;
            }
            return isValid;
        }
        public override void Execute()
        {
            color.Evaluate();
            string colorValue = (string)color.Value;
            // Actualizar el color del pincel
            switch (colorValue.ToLower())
            {
                case "red": Canvas.BrushColor = Colors.Red; break;
                case "blue": Canvas.BrushColor = Colors.Blue; break;
                case "green": Canvas.BrushColor = Colors.Green; break;
                case "yellow": Canvas.BrushColor = Colors.Yellow; break;
                case "black": Canvas.BrushColor = Colors.Black; break;
                case "white": Canvas.BrushColor = Colors.White; break;
                case "orange": Canvas.BrushColor = Colors.Orange; break;
                case "purple": Canvas.BrushColor = Colors.Purple; break;
                case "transparent": Canvas.BrushColor = Colors.Transparent; break;
                default: break;
            }
        }
    }
}