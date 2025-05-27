public class Scope
{
    public Dictionary<string, object> Value = new Dictionary<string, object>();
    private Dictionary<string, ExpressionType> Type = new Dictionary<string, ExpressionType>();
    public Scope()
    {}
    public object GetValue(string name)
    {
        if (Value.ContainsKey(name)) return Value[name];
        else return null;
    }
    public void SetValue(string name, object value)
    {
        if (Value.ContainsKey(name)) Value[name] = value;
        else Value.Add(name, value);
    }
    public ExpressionType GetType(string name)
    {
        if (Type.ContainsKey(name)) return Type[name];
        else return ExpressionType.ErrorType;
    }
    public void SetType(string name, ExpressionType type)
    {
        if (Type.ContainsKey(name)) Type[name] = type;
        else Type.Add(name, type);
    }
}
    