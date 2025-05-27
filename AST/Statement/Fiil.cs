
public class Fill : Statement
{
    public Fill(CodeLocation location) : base(location)
    { }
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
    public override void Execute(ExecutionContext context)
    {
        int startX = context.Canvas.Walle.ActualX;
        int startY = context.Canvas.Walle.ActualY;
        string targetColor = context.Canvas.GetPixel(startX, startY);
        context.Canvas.FillSpace(startX, startY, targetColor);
    }
}