public class Context
{
    public (int, int) Canvas { get; set; } // Estado del canvas
    public List<string> ValidColors { get; } = new List<string> { "Red", "Blue", "Green", "Yellow", "Orange", "Purple", "Black", "White", "Transparent" };

    public bool IsValidColor(string color) => ValidColors.Contains(color);
    public HashSet<string> Labels { get; } = new HashSet<string>();
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
}