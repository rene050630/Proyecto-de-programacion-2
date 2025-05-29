
public class Fill : Statement
{
    Canvas Canvas;
    public Fill(CodeLocation location, Canvas canvas) : base(location)
    {
        Canvas = canvas;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        bool isValid = true;
        // 2. Obtener posición actual
        int startX = context.Canvas.Walle.ActualX;
        int startY = context.Canvas.Walle.ActualY;
        int canvasSize = context.Canvas.Size;

        // 3. Chequeo semántico: posición válida
        if (startX < 0 || startX >= canvasSize || startY < 0 || startY >= canvasSize)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, "Wall-E is outside of the canvas"));
            isValid = false;
        }
        return isValid;
    }
    public override void Execute()
    {
        int startX = Canvas.Walle.ActualX;
        int startY = Canvas.Walle.ActualY;
        string targetColor = Canvas.GetPixel(startX, startY);
        Canvas.FillSpace(startX, startY, targetColor);
    }
}