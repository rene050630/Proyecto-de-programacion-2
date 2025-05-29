
public class Label : Expression
{
    public string Name;
    public override ExpressionType Type { get; set; }
    public override object? Value { get; set; }
    public Label(CodeLocation location, string name) : base(location)
    {
        Name = name;
    }
    public override bool checksemantic(Context context, List<CompilingError> errors)
    {
        return true;
    }
    public override void Evaluate()
    {

    }
    public override string ToString()
    {
        return $"{Name}:";
    }
}