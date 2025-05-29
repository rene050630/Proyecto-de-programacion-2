public class GetActualY : Statement
{
    Canvas Canvas;
    public GetActualY(CodeLocation location, Canvas canvas) : base(location)
    {
        Canvas = canvas;
    }
    public override bool checksemantic(Context conteYt, List<CompilingError> errors)
    {
        return true;
    }
    public override void Execute()
    {
        Canvas.GetActualY();
    }
}