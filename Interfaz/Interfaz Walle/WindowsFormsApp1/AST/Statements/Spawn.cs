using System.Collections.Generic;
using System;

namespace WindowsFormsApp1
{
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
            if (!X.checksemantic(context, errors) || X.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(X.location, ErrorCode.Invalid,
                    "X coordenate must be numeric"));
                return false;
            }

            if (!Y.checksemantic(context, errors) || Y.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(Y.location, ErrorCode.Invalid,
                    "Y coordenate must be numeric"));
                return false;
            }
            X.Evaluate();
            Y.Evaluate();
            if (Canvas.IsSpawnCalled)
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid,
                    "Spawn can only be called once"));
                return false;
            }
            if (Convert.ToInt32(X.Value) < 0 || Convert.ToInt32(X.Value) >= Canvas.Size || Convert.ToInt32(Y.Value) < 0 || Convert.ToInt32(Y.Value) >= Canvas.Size)
            {
                errors.Add(new CompilingError(location, ErrorCode.Invalid,
                    $"Invalid position ({X}, {Y}) for a {Canvas.Size}-sized canvas"));
                return false;
            }
            Canvas.IsSpawnCalled = true;
            return true;
        }
        public override void Execute()
        {
            X.Evaluate();
            Y.Evaluate();
            Canvas.Spawn(Convert.ToInt32(X.Value), Convert.ToInt32(Y.Value));
        }
    }
}