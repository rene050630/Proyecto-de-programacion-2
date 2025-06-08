using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class Bool : AtomsExpression
    {
        public Bool(bool value, CodeLocation location) : base(location)
        {
            Value = value;
        }
        public override object Value { get; set; }
        public override ExpressionType Type
        {
            get { return ExpressionType.Boolean; }
            set { }
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            return true;
        }
        public override void Evaluate()
        {

        }
        public override string ToString()
        {
            return string.Format("{0}", Value);
        }
    }
}