using System.Text;

public class ProgramNode : AST
{
    public List<Statement> Statements { get; }
    public Dictionary<string, Variable> Variables { get; }
    public List<CompilingError> Errors { get; set; }

    public ProgramNode(CodeLocation location) : base(location)
    {
        Statements = new List<Statement>();
        Variables = new Dictionary<string, Variable>();
        Errors = new List<CompilingError>();
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