
public class Label : Statement
{
    public string Name;
    public Label(CodeLocation location, string name) : base(location)
    {
        Name = name;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        return true;
    }
    public override void Execute()
    {

    }
    public override string ToString()
    {
        return $"{Name}:";
    }
}