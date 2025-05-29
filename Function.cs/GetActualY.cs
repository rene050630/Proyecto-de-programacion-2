public class GetActualY : Expression
{
    Canvas Canvas;
    public override ExpressionType Type { get; set; } = ExpressionType.Function;
    public override object? Value { get; set; }
    public GetActualY(CodeLocation location, Canvas canvas) : base(location)
    {
        Canvas = canvas;
    }
    public override bool checksemantic(Context conteYt, List<CompilingError> errors)
    {
        return true;
    }
    public override void Evaluate()
    {
        Canvas.GetActualY();
    }
}