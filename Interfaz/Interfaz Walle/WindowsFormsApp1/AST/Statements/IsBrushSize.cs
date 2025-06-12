using System.Collections.Generic;
using System;
using System.Linq.Expressions;

namespace WindowsFormsApp1
{
    public class IsBrushSize : Expression
    {
        Expression size;
        Canvas Canvas;
        public override object Value { get; set; }
        public override ExpressionType Type
        {
            get { return ExpressionType.Number; }
            set { }
        }
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
        public override void Evaluate()
        {
            size.Evaluate();
            Value = Canvas.IsBrushSize(Convert.ToInt32(size.Value));
        }
    }
}