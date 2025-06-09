
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Drawing;

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
            if (!color.checksemantic(context, errors) || color.Type != ExpressionType.Text)
            {
                errors.Add(new CompilingError(color.location, ErrorCode.Invalid, "Colors requieres to be strings"));
                return false;
            }
            color.Evaluate();
            string colorValue = color.Value.ToString().ToLower();
            if(!context.IsValidColor(colorValue))
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
            ConvertCanvasColor(Canvas.BrushColor);
        }
        public Color ConvertCanvasColor(Colors canvasColor)
        {
            switch (canvasColor)
            {
                case Colors.Blue: return Color.Blue;
                case Colors.Red: return Color.Red;
                case Colors.Green: return Color.Green;
                case Colors.Yellow: return Color.Yellow;
                case Colors.Black: return Color.Black;
                case Colors.White: return Color.White;
                case Colors.Purple: return Color.Purple;
                case Colors.Orange: return Color.Orange;
                case Colors.Transparent: return Color.Transparent;
                default: return Color.White;
            }
        }
    }
}