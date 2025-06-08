using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class Or : BinaryExpression
    {
        public override ExpressionType Type { get; set; }
        public override object Value { get; set; }
        public Or(CodeLocation location, Expression left, Expression right) : base(location)
        {
            Left = left;
            Right = right;
        }
        public override void Evaluate()
        {
            Right.Evaluate();
            Left.Evaluate();

            this.Value = (bool)this.Left.Value || (bool)this.Right.Value;
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            bool right = Right.checksemantic(context, errors);
            bool left = Left.checksemantic(context, errors);
            if ((Right.Type != ExpressionType.Boolean) || (Left.Type != ExpressionType.Boolean))
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid, "Operator '||' requires boolean operands"));
                Type = ExpressionType.ErrorType;
                return false;
            }
            Type = ExpressionType.Boolean;
            return right && left;
        }
    }
}