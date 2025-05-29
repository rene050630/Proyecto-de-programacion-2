public class GetCanvasSize : Statement
{
    Canvas Canvas;
    public GetCanvasSize(CodeLocation location, Canvas canvas) : base(location)
    {
        Canvas = canvas;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        return true;
    }
    public override void Execute()
    {
        Canvas.GetCanvasSize();
    }
}