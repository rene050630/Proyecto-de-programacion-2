using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WindowsFormsApp1
{
    public class ParenthesizedExpression : Expression
    {
        public Expression expression { get; private set; }
        public ParenthesizedExpression(CodeLocation location, Expression exp) : base(location)
        {
            this.expression = exp;
        }
        public override void Evaluate()
        {
            expression.Evaluate();
            this.Value = expression.Value;
        }
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            bool isValid = expression.checksemantic(context, errors);
            if (expression.Type == ExpressionType.Number)
            {
                this.Type = ExpressionType.Number;
                return isValid;
            }
            else if (expression.Type == ExpressionType.Text)
            {
                this.Type = ExpressionType.Text;
                return isValid;
            }
            else if (expression.Type == ExpressionType.Boolean)
            {
                this.Type = ExpressionType.Boolean;
                return isValid;
            }
            else
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid, "Invalid Expression"));
                this.Type = ExpressionType.ErrorType;
                return false;
            }
        }
        public override string ToString()
        {
            if (Value == null)
            {
                return String.Format("({0})", expression);
            }
            return Value.ToString();
        }
    }
}