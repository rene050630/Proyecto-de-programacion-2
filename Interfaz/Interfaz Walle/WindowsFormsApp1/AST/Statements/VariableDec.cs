using System.Collections.Generic;

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
            Operator = op;
            this.context = context;
        }
        public override void Execute()
        {
            Name.Evaluate();
            //if (Name is Variable)
            {
                if (Operator.value == TokenValues.Assign)
                {
                    context.SetValue(Name.ToString(), value.Value);
                    return;
                }
            }

        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            Name.Evaluate();
            Name.checksemantic(context, errors);
            value.Evaluate();
            // 1. Validar nombre de variable
            if (!IsIdentifier(Name.ToString()))
            {
                errors.Add(new CompilingError(
                    location,
                    ErrorCode.Invalid,
                    $"Variable name is invalid"
                ));
                return false;
            }
            // 2. Validar expresión
            bool ValidExpression = value.checksemantic(context, errors);

            return ValidExpression;
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