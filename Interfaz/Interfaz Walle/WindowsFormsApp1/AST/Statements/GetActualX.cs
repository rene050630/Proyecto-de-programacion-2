
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class GetActualX : Statement
    {
        Canvas Canvas;
        public GetActualX(CodeLocation location, Canvas canvas) : base(location)
        {
            Canvas = canvas;
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            return true;
        }
        public override void Execute()
        {
            Canvas.GetActualX();
        }
    }
}