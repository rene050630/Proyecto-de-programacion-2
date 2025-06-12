
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class GetActualX : Expression
    {
        Canvas Canvas;
        public override object Value { get; set; }
        public override ExpressionType Type
        {
            get { return ExpressionType.Number; }
            set { }
        }
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
            Value = Canvas.GetActualX();
        }
    }
}