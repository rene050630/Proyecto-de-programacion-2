using System.Collections.Generic;
using System.Linq.Expressions;

namespace WindowsFormsApp1
{
    public class ParenthesizedExpression : Expression
    {
        public Expression InnerExpression { get; }

        public ParenthesizedExpression(CodeLocation location, Expression inner)
            : base(location)
        {
            InnerExpression = inner;
        }

        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            bool group = InnerExpression.checksemantic(context, errors);
            if (InnerExpression.Type == ExpressionType.Number)
            {
                InnerExpression.Type = ExpressionType.Number;
                return group;
            }
            else if (InnerExpression.Type == ExpressionType.Text)
            {
                InnerExpression.Type = ExpressionType.Text;
                return group;
            }
            else if (InnerExpression.Type == ExpressionType.Boolean)
            {
                InnerExpression.Type = ExpressionType.Boolean;
                return group;
            }
            else
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid, "Expresion invalida"));
                return false;
            }
        }

        public override void Evaluate()
        {
            InnerExpression.Evaluate();
            Value = InnerExpression.Value;
        }
        //public override string ToString()
        //{
        //    if (Value == null)
        //    {
        //        return string.Format("({0})", InnerExpression);
        //    }
        //    return Value.ToString();
        //}
        public override ExpressionType Type { get; set; }
        public override object Value { get; set; }
    }
}