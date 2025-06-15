
using System.Collections.Generic;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace WindowsFormsApp1
{
    public class Div : BinaryExpression
    {
        public override ExpressionType Type { get; set; }
        public override object Value { get; set; }
        List<CompilingError> errors;
        public Div(CodeLocation location, Expression left, Expression right, List<CompilingError> errors) : base(location)
        {
            Left = left;
            Right = right;
            this.errors = errors;
        }
        public override void Evaluate()
        {
            Right.Evaluate();
            Left.Evaluate();
            if (Convert.ToInt32(Right.Value) == 0)
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid, "You can't divide by zero"));
                Type = ExpressionType.ErrorType;
                return;
            }
            Value = Convert.ToInt32(Left.Value) / Convert.ToInt32(Right.Value);
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            bool left = Left.checksemantic(context, errors);
            bool right = Right.checksemantic(context, errors);
            if (Right.Type != ExpressionType.Number || Left.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid, "You can only divide numbers"));
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
                return string.Format("({0} / {1})", Left, Right);
            }
            return Value.ToString();
        }
    }
}