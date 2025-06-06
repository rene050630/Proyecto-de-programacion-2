public class Context
{
    public Dictionary<string, object> Value = new Dictionary<string, object>();
    private Dictionary<string, ExpressionType> Type = new Dictionary<string, ExpressionType>();
    public List<string> ValidColors { get; } = new List<string> { "Red", "Blue", "Green", "Yellow", "Orange", "Purple", "Black", "White", "Transparent" };
    public bool IsValidColor(string color) => ValidColors.Contains(color);
    public List<string> Labels { get; } = new List<string>();
    private Dictionary<string, int> _labelPositions = new();
    private int _currentPosition;

    public void RegisterLabel(string label, int position)
    {
        _labelPositions[label] = position;
    }

    public void JumpToLabel(string label)
    {
        if (_labelPositions.TryGetValue(label, out int pos))
        {
            _currentPosition = pos;
        }
    }
    public bool LabelExists(string label)
    {
        return Labels.Contains(label);
    }
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
     public object Execute(string name)
    {
        if (Value.ContainsKey(name)) return Value[name];
        else return null;
    }
}