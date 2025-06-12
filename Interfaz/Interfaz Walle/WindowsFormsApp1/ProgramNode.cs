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
        private Dictionary<string, int> labelPositions = new Dictionary<string, int>();

        public ProgramNode(CodeLocation location, List<Statement> Statements) : base(location)
        {
            this.Statements = Statements;
            BuildLabelDictionary();
        }
        private void BuildLabelDictionary()
        {
            for (int i = 0; i < Statements.Count; i++)
            {
                if (Statements[i] is Label label)
                {
                    labelPositions[label.Name] = i;
                }
            }
        }
        public void Execute()
        {
            position = 0;
            while (position < Statements.Count)
            {
                Statements[position].Execute();

                if (Statements[position] is GoTo gotoStatement)
                {
                    if (gotoStatement.ShouldJump())
                    {
                        Console.WriteLine("entro aqui ");
                        if (labelPositions.TryGetValue(gotoStatement.Label, out int targetPosition))
                        {
                            Console.WriteLine(gotoStatement.Label);
                            position = targetPosition;
                            continue;
                        }
                    }
                }

                position++;
            }
        }
        public override bool checksemantic(Context context, List<CompilingError> errors)
        {
            bool isValid = true;
            if (Statements.Count == 0 || !(Statements[0] is Spawn))
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid, "This program have to start with a Spawn"));
                isValid = false;
            }
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