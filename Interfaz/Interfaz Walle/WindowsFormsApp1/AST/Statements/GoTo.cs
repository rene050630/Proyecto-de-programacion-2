using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class GoTo : Statement
    {
        public string Label { get; }
        public Expression Condition { get; }
        public GoTo(CodeLocation location, string label, Expression condition) : base(location)
        {
            Label = label;
            Condition = condition;
        }
        public bool ShouldJump()
        {
            Condition.Evaluate();
            return (bool)Condition.Value;
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            if (!context.LabelExists(Label))
            {
                errors.Add(new CompilingError(location, ErrorCode.UndefinedLabel, $"Label '{Label}' undefined"));
                return false;
            }
            if (!Condition.checksemantic(context, errors) ||
                Condition.Type != ExpressionType.Boolean)
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid, "Condition requires to be booelan"));
                return false;
            }
            return true;
        }
        public override void Execute()
        {
        

        }
    }
}