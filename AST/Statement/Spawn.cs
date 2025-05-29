public class Spawn : Statement
{
    Expression X;
    Expression Y;
    ExecutionContext Context;
    Canvas Canvas;
    public Spawn(CodeLocation location, Expression x, Expression y, Canvas canvas) : base(location)
    {
        X = x;
        Y = y;
        Canvas = canvas;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        bool isValid = true;
        // 1. Validar que las expresiones X e Y sean numéricas
        if (!X.checksemantic(context, errors) || X.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(X.location, ErrorCode.Invalid,
                "Coordenada X debe ser numérica"));
            isValid = false;
        }

        if (!Y.checksemantic(context, errors) || Y.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(Y.location, ErrorCode.Invalid,
                "Coordenada Y debe ser numérica"));
            isValid = false;
        }
        if (Convert.ToInt32(X.Value) < 0 || Convert.ToInt32(X.Value) >= context.Canvas.Size || Convert.ToInt32(Y.Value) < 0 || Convert.ToInt32(Y.Value) >= context.Canvas.Size)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid,
                $"Posición inválida ({X}, {Y}) para canvas de tamaño {context.Canvas.Size}"));
            isValid = false;
        }
        // 2. Validar que Spawn no se haya llamado antes (si aplica)
         if (Canvas.IsSpawnCalled)
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, 
                "Spawn solo puede llamarse una vez"));
            isValid = false;
        }
        return isValid;
    }
    public override void Execute()
    {
        // Actualizar estado
        Canvas.Spawn(Convert.ToInt32(X.Value), Convert.ToInt32(Y.Value));
        Canvas.IsSpawnCalled = true; // Marcar como invocado
    }
}