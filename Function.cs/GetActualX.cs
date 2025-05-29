
public class GetActualX : Expression
{
    Canvas Canvas;
    public override ExpressionType Type { get; set; } = ExpressionType.Function;
    public override object? Value { get; set; }
    public GetActualX(CodeLocation location, Canvas canvas) : base(location)
    {
        Canvas = canvas;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        return true;
    }
    public override void Evaluate()
    {
        Canvas.GetActualX();
    }
}