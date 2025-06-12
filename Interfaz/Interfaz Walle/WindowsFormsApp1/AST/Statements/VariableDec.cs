using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Win32;

namespace WindowsFormsApp1
{
    public class VariableDec : Statement
    {
        public Expression Name { get; }
        public Expression value { get; set; }
        Context context;
        public VariableDec(Expression Name, Expression value, CodeLocation location, Context context)
            : base(location)
        {
            this.Name = Name;
            this.value = value;
            this.context = context;
        }
        public override void Execute()
        {   
            value.Evaluate();
            context.SetValue(Name.ToString(), value.Value);
            return;
        
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            value.checksemantic(context, errors);
            context.SetType(Name.ToString(), value.Type);
            return true;
        }
        public override string ToString()
        {
            return $"{Name}<-{value}";
        }
    }
}