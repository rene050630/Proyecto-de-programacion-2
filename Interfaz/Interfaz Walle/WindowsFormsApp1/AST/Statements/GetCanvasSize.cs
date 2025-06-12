using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class GetCanvasSize : Expression
    {
        Canvas Canvas;
        public override object Value { get; set; }
        public override ExpressionType Type
        {
            get { return ExpressionType.Number; }
            set { }
        }
        public GetCanvasSize(CodeLocation location, Canvas canvas) : base(location)
        {
            Canvas = canvas;
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            return true;
        }
        public override void Evaluate()
        {
            Value = Canvas.GetCanvasSize();
        }
    }
}