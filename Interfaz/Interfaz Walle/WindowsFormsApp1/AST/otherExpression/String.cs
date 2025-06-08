using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class Text : Expression
    {
        public override object Value { get; set; }
        public override ExpressionType Type { get; set; }
        public string Substring;
        public Text(string value, CodeLocation location) : base(location)
        {
            Value = value;
        }
        public override void Evaluate()
        {
            if (Value is string stringValue)
            {
                Substring = stringValue.Substring(1, stringValue.Length - 2);
            }
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            Type = ExpressionType.Text;
            return true;
        }
    }
}