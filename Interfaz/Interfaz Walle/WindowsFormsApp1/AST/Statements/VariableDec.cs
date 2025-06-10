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
        Token Operator;
        Context context;
        public VariableDec(Expression Name, Expression value, Token op, CodeLocation location, Context context)
            : base(location)
        {
            this.Name = Name;
            this.value = value;
            this.Operator = op;
            this.context = context;
        }
        public override void Execute()
        {
            Console.WriteLine("value :" + value.Value);
            context.SetValue(Name.ToString(), value.Value);
            return;
        
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            value.checksemantic(context, errors);
            context.SetType(Name.ToString(), value.Type);
            return true;
        }

        private bool IsIdentifier(string nombre)
        {
            // Implementar reglas del lenguaje:
            // - No puede comenzar con número o _
            // - Solo letras, números y _
            if (string.IsNullOrEmpty(nombre)) return false;
            if (char.IsDigit(nombre[0])) return false;
            if (nombre[0] == '_') return false;

            foreach (char c in nombre)
            {
                if (!char.IsLetterOrDigit(c) && c != '_')
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"{Name}<-{value}";
        }
    }
}