
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class Fill : Statement
    {
        Canvas Canvas;
        public Fill(CodeLocation location, Canvas canvas) : base(location)
        {
            Canvas = canvas;
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            int startX = Canvas.ActualX;
            int startY = Canvas.ActualY;
            int canvasSize = Canvas.Size;
            if (startX < 0 || startX >= canvasSize || startY < 0 || startY >= canvasSize)
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid, "Wall-E is outside of the canvas"));
                return false;
            }
            return true;
        }
        public override void Execute()
        {
            int startX = Canvas.ActualX;
            int startY = Canvas.ActualY;
            Colors targetColor = Canvas.GetPixel(startX, startY);
            Canvas.FillSpace(startX, startY, targetColor);
        }
    }
}