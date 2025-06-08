using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public abstract class AST
    {
        public CodeLocation location;
        public abstract bool checksemantic(Context context, List<CompilingError> errors);
        public AST(CodeLocation Location)
        {
            location = Location;
        }
    }
}