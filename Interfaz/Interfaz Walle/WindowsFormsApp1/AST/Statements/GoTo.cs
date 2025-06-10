using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class GoTo : Statement
    {
        public Label Label { get; }
        public Expression Condition { get; }
        public GoTo(CodeLocation location, Label label, Expression condition) : base(location)
        {
            Label = label;
            Condition = condition;
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            if (!context.LabelExists(Label.Name))
            {
                errors.Add(new CompilingError(location, ErrorCode.UndefinedLabel, $"Label '{Label.Name}' undefined"));
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
            Condition.Evaluate();
            if (Condition is Bool condition && (bool)condition.Value)
            {
                
            }

        }
    }
}