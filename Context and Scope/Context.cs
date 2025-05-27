public class Context   
{
    public Canvas Canvas { get; set; } // Estado del canvas
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
public class ExecutionContext //Eliminar esta clase y sustituir ExecutionContext context por Canvas canvas
{
    public Canvas Canvas { get; } = new Canvas(256); // Tamaño por defecto
    public string BrushColor { get; set; } = "Transparent";
    public bool IsSpawnCalled = false;
    public int BrushSize { get; set; } = 1;

    public List<string> ValidColors { get; } = new List<string> 
    { 
        "Red", "Blue", "Green", "Yellow", 
        "Orange", "Purple", "Black", "White", "Transparent" 
    };

    public bool IsValidColor(string? color) 
        => color != null && ValidColors.Contains(color);
}