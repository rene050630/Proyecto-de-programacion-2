using System.Collections.Generic;
using System;

namespace WindowsFormsApp1
{
    public class Add : BinaryExpression
    {
        public override ExpressionType Type { get; set; }
        public override object Value { get; set; }
        public Add(CodeLocation location, Expression left, Expression right) : base(location)
        {
            Left = left;
            Right = right;
        }
        public override void Evaluate()
        {
            Left.Evaluate();
            Right.Evaluate();

            Value = Convert.ToInt32(Left.Value) + Convert.ToInt32(Right.Value);
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            bool right = Right.checksemantic(context, errors);
            bool left = Left.checksemantic(context, errors);
            if (Right.Type != ExpressionType.Number || Left.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid, "Operation + requires number operands"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.Number;
            return right && left;
        }
        public override string ToString()
        {
            if (Value == null)
            {
                return string.Format("({0} + {1})", Left, Right);
            }
            return Value.ToString();
        }
    }
}