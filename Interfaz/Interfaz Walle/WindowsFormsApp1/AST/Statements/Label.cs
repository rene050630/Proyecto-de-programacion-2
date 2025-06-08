
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public class Label : Statement
    {
        public string Name;
        public Label(CodeLocation location, string name) : base(location)
        {
            Name = name;
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            if (!IsIdentifier(Name))
            {
                errors.Add(new CompilingError(
                    location,
                    ErrorCode.Invalid,
                    $"Label name is invalid"
                ));
                return false;
            }
            context.RegisterLabel(Name, location.Line);
            context.Labels.Add(Name);
            return true;
        }
        public override void Execute()
        {

        }
        public override string ToString()
        {
            return $"{Name}";
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
    }
}