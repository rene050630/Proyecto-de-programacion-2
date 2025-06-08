using System.Linq.Expressions;

public class IsBrushSize : Statement
{
    Expression size;
    Canvas Canvas;
    public IsBrushSize(CodeLocation location, Expression size, Canvas canvas) : base(location)
    {
        this.size = size;
        Canvas = canvas;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        bool isValid = true;
        if (!size.checksemantic(context, errors) || size.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(size.location, ErrorCode.Invalid, "Size requires to be a number"));
            isValid = false;
        }
        return isValid;
    }
    public override void Execute()
    {
        size.Evaluate();
        Canvas.IsBrushSize(Convert.ToInt32(size.Value));
    }
}