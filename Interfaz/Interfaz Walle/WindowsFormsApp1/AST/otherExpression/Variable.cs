//Error en GetType
using System;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class Variable : Expression
    {
        public string variable;
        public override ExpressionType Type { get; set; }
        public override object Value { get; set; }
        private Context context;
        public Variable(CodeLocation location, string variable, Context context) : base(location)
        {
            this.variable = variable;
            this.context = context;
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            foreach(var x in context.Type)
            {
                Console.WriteLine(x.Key);
            }
            if (context.GetType(variable) == ExpressionType.ErrorType)
            {
                errors.Add(new CompilingError(location, ErrorCode.UndefinedLabel, "Variable is undefined"));
                Type = ExpressionType.ErrorType;
                return false;
            }
            Type = context.GetType(variable);
            return true;
        }
        public override void Evaluate()
        {
            this.Value = context.Execute(variable);
            Console.WriteLine("entra aqui " + Value);
        }
        public override string ToString()
        {
            return String.Format("{0}", variable);
        }
    }
}