
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class Number : AtomsExpression
    {

        public Number(int value, CodeLocation location) : base(location)
        {
            this.Value = value;
        }
        public override object Value { get; set; }
        public override ExpressionType Type
        {
            get { return ExpressionType.Number; }
            set { }
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            return true;
        }
        public override void Evaluate()
        {

        }
    }
}