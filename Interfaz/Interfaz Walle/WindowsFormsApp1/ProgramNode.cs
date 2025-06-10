using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsFormsApp1
{
    public class ProgramNode : AST
    {
        public List<Statement> Statements { get; }
        public Canvas Canvas { get; }
        public int position = 0;

        public ProgramNode(CodeLocation location, List<Statement> Statements) : base(location)
        {
            this.Statements = Statements;
        }
        public void Execute()
        {
            foreach (Statement item in Statements)
            {
                item.Execute();
            }
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            bool isValid = true;
            // Verificar que existe un comando Spawn al inicio
            if (Statements.Count == 0 || !(Statements[0] is Spawn))
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid, "This program have to start with a Spawn"));
                isValid = false;
            }
            // Verificar semántica de cada sentencia
            foreach (var statement in Statements)
            {
                if (!statement.checksemantic(context, errors))
                {
                    isValid = false;
                }
            }
            return isValid;
        }
    }
}