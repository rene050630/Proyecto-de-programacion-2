
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;

namespace WindowsFormsApp1
{
    public class Label : Statement
    {
        public string Name;
        public Context Context;
        public Label(CodeLocation location, string name, Context context) : base(location)
        {
            Name = name;
            Context = context;
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            if (!context.LabelExists(Name))
            {
                errors.Add(new CompilingError(location, ErrorCode.UndefinedLabel, $"Label '{Name}' not found"));
                return false;
            }
            return true;
        }
        public override void Execute()
        {
            
        }
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}