
using System.Collections.Generic;
using System;

namespace WindowsFormsApp1
{
    public class Subs : BinaryExpression
    {
        public override ExpressionType Type { get; set; }
        public override object Value { get; set; }
        public Subs(CodeLocation location, Expression left, Expression right) : base(location)
        {
            Left = left;
            Right = right;
        }
        public override void Evaluate()
        {
            Right.Evaluate();
            Left.Evaluate();

            Value = Convert.ToInt32(Right.Value) - Convert.ToInt32(Left.Value);
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            bool right = checksemantic(context, errors);
            bool left = checksemantic(context, errors);
            if (Right.Type != ExpressionType.Number && Left.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid, "Substract operation requires numeric operands"));
                Type = ExpressionType.ErrorType;
                return false;
            }
            Type = ExpressionType.Number;
            return left && right;
        }
        public override string ToString()
        {
            if (Value == null)
            {
                return string.Format("{0} - {1}", Left, Right);
            }
            return Value.ToString();
        }
    }
}