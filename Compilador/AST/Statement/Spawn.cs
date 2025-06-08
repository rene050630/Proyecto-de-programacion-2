public class Spawn : Statement
{
    Expression X;
    Expression Y;
    Canvas Canvas;
    public Spawn(CodeLocation location, Expression x, Expression y, Canvas canvas) : base(location)
    {
        X = x;
        Y = y;
        Canvas = canvas;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        X.Evaluate();
        Y.Evaluate();
        // 1. Validar que las expresiones X e Y sean numéricas
        if (!X.checksemantic(context, errors) || X.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(X.location, ErrorCode.Invalid,
                "Coordenada X debe ser numérica"));
            return false;
        }

        if (!Y.checksemantic(context, errors) || Y.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(Y.location, ErrorCode.Invalid,
                "Coordenada Y debe ser numérica"));
            return false;
        }
        if (Convert.ToInt32(X.Value) < 0 || Convert.ToInt32(X.Value) >= Canvas.Size || Convert.ToInt32(Y.Value) < 0 || Convert.ToInt32(Y.Value) >= Canvas.Size)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid,
                $"Posición inválida ({X}, {Y}) para canvas de tamaño {Canvas.Size}"));
            return false;
        }
        // 2. Validar que Spawn no se haya llamado antes (si aplica)
        if (Canvas.IsSpawnCalled)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid,
                "Spawn solo puede llamarse una vez"));
            return false;
        }
        Canvas.IsSpawnCalled = true;
        return true;
    }
    public override void Execute()
    {
        X.Evaluate();
        Y.Evaluate();
        // Actualizar estado
        Canvas.Spawn(Convert.ToInt32(X.Value), Convert.ToInt32(Y.Value));
        //Canvas.IsSpawnCalled = true; // Marcar como invocado
    }
}