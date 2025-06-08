using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class GoTo : Statement
    {
        public Label Label { get; }
        public Expression Condition { get; }
        private Context Context;
        public GoTo(CodeLocation location, Label label, Expression condition, Context context) : base(location)
        {
            Label = label;
            Condition = condition;
            Context = context;
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            Condition.Evaluate();
            Label.Execute();
            if (!context.LabelExists(Label.Name))
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
            // Evaluar la condición
            Condition.Evaluate();
            if (Condition is Bool condition && (bool)condition.Value)
            {
                Context.JumpToLabel(Label.Name);
            }

        }
    }
}